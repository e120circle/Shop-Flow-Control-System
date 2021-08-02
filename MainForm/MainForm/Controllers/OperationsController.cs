using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MainForm.Areas.Identity.Data;
using MainForm.Models.Operations;
using MainForm.ViewModels.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using SQLClass.Models.Item;
using SQLClass.Models.Job;
using SQLClass.Models.OperationsDetail;
using SQLClass.Models.OperationsSubmit;
using SQLClass.Models.Routing;
using SQLClass.Models.SysCommon;
using SQLClass.Models.OperationsResource;
using SQLClass.Models.Users;

namespace MainForm.Controllers
{
    public class OperationsController : Controller
    {
        public static OperationsModel OperationsModel { get; set; }

        public static OperationsViewModel OperationsViewModel { get; set; }

        private readonly ILogger<OperationsController> _logger;

        private readonly IMapper _mapper;

        public OperationsController(IOperationsSubmitManage SubmitContext, IOperationsDetailManage DetailContext, ILogger<OperationsController> logger
            , IMapper mapper, IJobManage JobContext, IRoutingManage RoutingManage, IUsersManage UsersManage,
            IItemManage ItemContext, IOperationsResourceManage ResourceContext , ISysCommonManage SysCommonContext)
        {
            _logger ??= logger;
            _mapper ??= mapper;

            OperationsModel ??= new OperationsModel(JobContext, RoutingManage, UsersManage, SubmitContext, DetailContext,
                ResourceContext, SysCommonContext);

            OperationsViewModel ??= new OperationsViewModel();
            OperationsViewModel.SubmitViewModel ??= new OperationsSubmitViewModel();
        }

        ///////////////////////////////////////////////////////////for start///////////////////////////////////////////////////////////
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Start(string Job_no_filter, int? page = 1, string sort = "Routing_no")
        {
            try
            {
                OperationsModel.GetJob(Job_no_filter);
                OperationsModel.GetRoutingByJob_no(Job_no_filter);
                OperationsViewModel = _mapper.Map<OperationsViewModel>(OperationsModel);

                OperationsViewModel.SubmitList = new List<OperationsSubmit>();
                foreach (Routing a in OperationsModel.RoutingList)
                {
                    OperationsViewModel.SubmitList.AddRange(OperationsModel.GetSubmitLike(a.Routing_no));
                }

                OperationsViewModel.RoutingListInPaging = PagingList.Create(OperationsViewModel.RoutingList, 20, (int)page, sort, "Routing_no");
                OperationsViewModel.RoutingListInPaging.RouteValue = new RouteValueDictionary {
                    { "Job_no_filter", Job_no_filter}
                };                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(OperationsViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{groupe}")]
        public async Task<IActionResult> GetGroupeUser(string groupe)
        {   //show groupe user
            List<SQLClass.Models.Users.Users> temp = new List<Users>();

            try
            {
                temp = OperationsModel.GetGroupeUser(groupe);
            }   
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Json(temp);
        }

        [Route("[controller]/[action]/{job_id}/{routing_no}")]
        public async Task<IActionResult> CreateOperations(int job_id, string routing_no, string Resource_no_filter)
        {   //show create operations
            try
            {
                OperationsModel.GetCreateOperations(job_id, routing_no);
                OperationsViewModel = _mapper.Map<OperationsViewModel>(OperationsModel);

                OperationsViewModel.SelectMachine = new List<OperationsResource>();
                OperationsModel.GetAllMachine();
                OperationsViewModel.MachineResourceInPaging = PagingList.Create(OperationsModel.MachineResource, 20, 1, "Resource_no", "Resource_no");
                OperationsViewModel.MachineResourceInPaging.RouteValue = new RouteValueDictionary {
                    { "Resource_no_filter", Resource_no_filter}
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(OperationsViewModel));
        }

        [HttpPost]
        [ActionName("SaveCreateSubmit")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateOperations(IFormCollection data)
        {   //save create operations
            try
            {
                if (ModelState.IsValid)
                {
                    OperationsModel.PostCreateOperations(data, OperationsViewModel.SelectMachine);
                    OperationsViewModel = _mapper.Map<OperationsViewModel>(OperationsModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            //var result = new { Redirect = Url.Action("Start", "Operations"), Job_no_filter = data["WorkViewModel.Job_no"] };
            //return Json(result);
            return await Task.Run(() => RedirectToAction("Start", new { Job_no_filter = data["WorkViewModel.Job_no"] }));
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public IActionResult GetJob_no(string term)
        {   //Item_no autocomplete
            try
            {
                OperationsModel.GetAllJob();
                if (term == null) term = "";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Json((from N in OperationsModel.JobList
                         where N.Job_no.Contains(term)
                         select new { N.Job_no })
                        );
        }

        [HttpPost]
        [Route("[controller]/[action]/{routing_no}")]
        public IActionResult GetSubmitLike(string routing_no)
        {   //edit operations
            List<SQLClass.Models.OperationsSubmit.OperationsSubmit> submit = new List<OperationsSubmit>();
            List<SQLClass.Models.OperationsDetail.OperationsDetail> detail = new List<OperationsDetail>();
            List<SelectListItem> type = new List<SelectListItem>();
            try
            {
                if (ModelState.IsValid)
                {
                    submit = OperationsModel.GetSubmitLike(routing_no);
                    detail = OperationsModel.GetDetail(submit);
                    type = OperationsModel.GetSubmitType();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            var result = new {routing_no = routing_no, submit = submit, detail = detail, type = type };
            return Json(result);
        }

        public async Task<IActionResult> GetAllMachine(string Resource_no_filter, int? page = 1, string sort = "Resource_no")
        {   //show all machine
            try
            {
                OperationsModel.GetAllMachine();

                OperationsViewModel.Resource_Sort_str = sort;
                if (!string.IsNullOrWhiteSpace(Resource_no_filter))
                {
                    OperationsModel.MachineResource = OperationsModel.MachineResource.Where(f => f.Resource_no.Contains(Resource_no_filter)).ToList();
                }
                OperationsViewModel.MachineResourceInPaging = PagingList.Create(OperationsModel.MachineResource, 20, (int)page, sort, "Resource_no");
                OperationsViewModel.MachineResourceInPaging.Action = "GetAllMachine";
                OperationsViewModel.MachineResourceInPaging.RouteValue = new RouteValueDictionary {
                    { "Resource_no_filter", Resource_no_filter}
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectMachineModalPartial", OperationsViewModel));
        }

        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> SelectMachine(string no)
        {   //select machine
            try
            {
                var tt = OperationsModel.GetResource(no);
                OperationsViewModel.SelectMachine ??= new List<OperationsResource>();

                if (!OperationsViewModel.SelectMachine.Any(x => x.Resource_no == tt.Resource_no))
                {
                    OperationsViewModel.SelectMachine.Add(tt);
                    OperationsViewModel.SelectMachineStr += tt.Resource_no;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectMachinePartial", OperationsViewModel));
        }

        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> RemoveSelectMachine(string no)
        {   //remove select machine
            try
            {
                OperationsViewModel.SelectMachine.Remove(OperationsViewModel.SelectMachine.Find(x => x.Resource_no == no));
                OperationsViewModel.SelectMachineStr = "";
                
                foreach (OperationsResource a in OperationsViewModel.SelectMachine)
                {
                    OperationsViewModel.SelectMachineStr += a.Resource_no;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectMachinePartial", OperationsViewModel));
        }
        ///////////////////////////////////////////////////////////for finish///////////////////////////////////////////////////////////
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Finish(string Job_no_filter, int? page = 1, string sort = "Routing_no")
        {
            try
            {
                OperationsModel.GetJob(Job_no_filter);
                OperationsModel.GetRoutingByJob_no(Job_no_filter);
                OperationsViewModel = _mapper.Map<OperationsViewModel>(OperationsModel);

                OperationsViewModel.SubmitList = new List<OperationsSubmit>();
                foreach (Routing a in OperationsModel.RoutingList)
                {
                    OperationsViewModel.SubmitList.AddRange(OperationsModel.GetSubmitLike(a.Routing_no));
                }

                OperationsViewModel.RoutingListInPaging = PagingList.Create(OperationsViewModel.RoutingList, 20, (int)page, sort, "Routing_no");
                OperationsViewModel.RoutingListInPaging.RouteValue = new RouteValueDictionary {
                    { "Job_no_filter", Job_no_filter}
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(OperationsViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{job_id}/{routing_no}/{submit_no}")]
        public async Task<IActionResult> EditOperations(int job_id, string routing_no, string submit_no, string Resource_no_filter)
        {   //show create operations
            try
            {
                OperationsModel.GetEditOperations(job_id, routing_no, submit_no);
                OperationsViewModel = _mapper.Map<OperationsViewModel>(OperationsModel);

                OperationsViewModel.ToolDetailList = new List<OperationsDetail>();
                OperationsModel.GetAllTool();
                OperationsViewModel.ToolResourceInPaging = PagingList.Create(OperationsModel.ToolResource, 10, 1, "Resource_no", "Resource_no");
                OperationsViewModel.ToolResourceInPaging.RouteValue = new RouteValueDictionary {
                    { "Resource_no_filter", Resource_no_filter}
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(OperationsViewModel));
        }

        [HttpPost]
        [ActionName("SaveEditSubmit")]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> EditOperations(string no, IFormCollection data)
        {   //save create operations
            try
            {
                if (ModelState.IsValid)
                {
                    for (int i = 0; i < OperationsViewModel.ToolDetailList.Count; i++)
                    {
                        OperationsViewModel.ToolDetailList[i].Operations_resource_description = data["tool.Operations_resource_description"][i];
                        OperationsViewModel.ToolDetailList[i].Reserved_field01 = Convert.ToInt32(data["tool.Reserved_field01"][i]);
                    }

                    OperationsModel.PostEditOperations(no, data, OperationsViewModel.ToolDetailList);
                }

                OperationsViewModel = _mapper.Map<OperationsViewModel>(OperationsModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            //var result = new { Redirect = Url.Action("Finish", "Operations"), Job_no_filter = data["WorkViewModel.Job_no"] };
            //return Json(result);
            return await Task.Run(() => RedirectToAction("Finish", new { Job_no_filter = data["WorkViewModel.Job_no"] }));
        }

        public async Task<IActionResult> GetAllTool(string Resource_no_filter, int? page = 1, string sort = "Resource_no")
        {   //show all tool
            try
            {
                OperationsModel.GetAllTool();

                OperationsViewModel.Resource_Sort_str = sort;
                if (!string.IsNullOrWhiteSpace(Resource_no_filter))
                {
                    OperationsModel.ToolResource = OperationsModel.ToolResource.Where(f => f.Resource_no.Contains(Resource_no_filter)).ToList();
                }
                OperationsViewModel.ToolResourceInPaging = PagingList.Create(OperationsModel.ToolResource, 20, (int)page, sort, "Resource_no");
                OperationsViewModel.ToolResourceInPaging.Action = "GetAllTool";
                OperationsViewModel.ToolResourceInPaging.RouteValue = new RouteValueDictionary {
                    { "Resource_no_filter", Resource_no_filter}
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectToolModalPartial", OperationsViewModel));
        }

        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> SelectTool(string no)
        {   //select tool
            try
            {
                var tt = OperationsModel.GetResource(no);
                OperationsViewModel.ToolDetailList ??= new List<OperationsDetail>();

                OperationsViewModel.ToolDetailList.Add(new OperationsDetail()
                {
                    Operations_resource_no = tt.Resource_no,
                    Operations_resource_type = tt.Operations_resource_type,
                    Operations_resource_name = tt.Resource_name,
                    Operations_resource_description = tt.Resource_description
                });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectToolPartial", OperationsViewModel));
        }

        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> RemoveSelectTool(string no)
        {   //remove select tool
            try
            {
                OperationsViewModel.ToolDetailList.Remove(OperationsViewModel.ToolDetailList.Find(x => x.Operations_resource_no == no));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectToolPartial", OperationsViewModel));
        }
    }
}
