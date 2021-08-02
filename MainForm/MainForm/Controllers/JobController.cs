using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MainForm.Areas.Identity.Data;
using MainForm.Models.Job;
using MainForm.ViewModels.Job;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLClass.Models.Company;
using SQLClass.Models.Job;
using SQLClass.Models.Routing;
using Microsoft.AspNetCore.Http;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Item;
using SQLClass.Models.ItemUOM;
using SQLClass.Models.AutoNumber;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainForm.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        public static JobModel Model { get; set; }

        public static JobViewModel ViewModel { get; set; }

        private readonly ILogger<JobController> _logger;

        private readonly IMapper _mapper;

        public JobController(IJobManage JobContext, IRoutingManage RoutingContext, ILogger<JobController> logger, ISysCommonManage SysCommonContext,
            IItemManage ItemContext, IItemUOMManage ItemUOMContext, IAutoNumber AutoNumberContext,  IMapper mapper, 
            UserManager<MainFormUsers> userManager, ICompanyManage company)
        {
            _logger ??= logger;
            _mapper ??= mapper;
            _userManager ??= userManager;

            Model ??= new JobModel(JobContext, RoutingContext, SysCommonContext, 
                ItemContext, ItemUOMContext, AutoNumberContext, company);
            ViewModel ??= new JobViewModel();
            ViewModel.WorkViewModel ??= new WorkViewModel();
            ViewModel.RoutingViewModel ??= new RoutingViewModel();
        }

        private UserManager<MainFormUsers> _userManager { get; set; }

        public async Task<IActionResult> Index(string Job_no_filter, string Item_no_filter, int Job_status_filter,
            int? page = 1, string sort = "-Job_no")
        {
            try
            {
                Model.GetAllJob();

                ViewModel = _mapper.Map<JobViewModel>(Model);

                ViewModel.Sort_str = sort;

                if (!string.IsNullOrWhiteSpace(Job_no_filter))
                {   //搜尋流程單號
                    ViewModel.JobList = ViewModel.JobList.Where(f => f.Job_no.Contains(Job_no_filter)).ToList();
                }

                if (!string.IsNullOrWhiteSpace(Item_no_filter))
                {   //搜尋料號
                    ViewModel.JobList = ViewModel.JobList.Where(f => f.Item_no.Contains(Item_no_filter)).ToList();
                }

                if (Job_status_filter != 0)
                {   //搜尋流程單狀態
                    ViewModel.JobList = ViewModel.JobList.Where(f => f.Job_status.Equals(Job_status_filter)).ToList();
                    var tt = Model.JobStatusFilterList.Select(f => f.Value == Job_status_filter.ToString()).ToList();
                    for(int i = 0; i < ViewModel.JobStatusFilterList.Count; i++)
                    {
                        ViewModel.JobStatusFilterList[i].Selected = tt[i];
                    }
                }

                ViewModel.JobListInPaging = PagingList.Create(ViewModel.JobList, 10, (int)page, sort, "-Job_no");
                ViewModel.JobListInPaging.RouteValue = new RouteValueDictionary {
                    { "Job_no_filter", Job_no_filter},
                    { "Item_no_filter", Item_no_filter},
                    { "Job_status_filter", Job_status_filter}
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        ///////////////////////////////////////////////////////////for job///////////////////////////////////////////////////////////
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateJob()
        {   //show create job
            try
            {
                Model.EmployeeUserList = new List<SelectListItem>();
                foreach (MainFormUsers a in _userManager.Users)
                {
                    if(a.is_enable == 1)
                        Model.EmployeeUserList.Add(new SelectListItem() { Value = a.Id, Text = a.Name + " " + a.ChinessName, Selected = false });
                }
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetCreateJob(user);

                ViewModel = _mapper.Map<JobViewModel>(Model);
                ItemPaging("", "", "", 1, "Item_no");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateJob(IFormCollection data)
        {   //create job
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    Model.PostCreateJob(data, user);
                    Model.GetEditJob(data["WorkViewModel.Job_no"], user);
                    ViewModel = _mapper.Map<JobViewModel>(Model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("EditJob", new { no = Model.WorkViewModel.Job_no }));
        }

        [HttpGet]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> EditJob(string no, int? page = 1, string sort = "Routing_no")
        {   //show edit job
            try
            {
                Model.EmployeeUserList = new List<SelectListItem>();
                foreach (MainFormUsers a in _userManager.Users)
                {
                    if (a.is_enable == 1)
                        Model.EmployeeUserList.Add(new SelectListItem() { Value = a.Id, Text = a.Name + " " + a.ChinessName, Selected = false });
                }
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetEditJob(no, user);
                ViewModel = _mapper.Map<JobViewModel>(Model);

                ItemPaging("", "", "", 1, "Item_no");

                ViewModel.Routing_Sort_str = sort;
                ViewModel.RoutingListInPaging = PagingList.Create(ViewModel.RoutingList, 20, (int)page, sort, "Routing_no");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View("EditJob", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> EditJob(string no, IFormCollection data)
        {   //edit job
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    Model.PostEditJob(no, data, user);
                    Model.GetEditJob(no, user);
                    ViewModel = _mapper.Map<JobViewModel>(Model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("EditJob", new { no = data["WorkViewModel.Job_no"] }));
            //return await Task.Run(() => PartialView("_EditJobPartial", ViewModel));
        }        

        [HttpPost]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> DeleteJob(string no)
        {   //delete job
            try
            {
                if (ModelState.IsValid)
                {
                    Model.DisableJob(no);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> ReleasedJob(string no)
        {   //delete job
            try
            {
                if (ModelState.IsValid)
                {
                    Model.ReleasedJob(no);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> ClosedJob(string no)
        {   //delete job
            try
            {
                if (ModelState.IsValid)
                {
                    Model.ClosedJob(no);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [Route("[controller]/[action]")]    
        public async Task<IActionResult> GetAllItem(string Item_no_filter, string Item_name_filter, string Item_description_filter, int? page = 1, string sort = "Item_no")
        {   //Get Item
            try
            {
                Model.GetAllItem();
                ViewModel = _mapper.Map<JobViewModel>(Model);

                ItemPaging(Item_no_filter, Item_name_filter, Item_description_filter, page, sort);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectItemModalPartial", ViewModel));
        }

        private void ItemPaging(string Item_no_filter, string Item_name_filter, string Item_description_filter, 
            int? page = 1, string sort = "Item_no")
        {
            ViewModel.Item_Sort_str = sort;
            
            if (!string.IsNullOrWhiteSpace(Item_no_filter))
            {   //搜尋料號
                ViewModel.ItemList = ViewModel.ItemList.Where(f => f.Item_no.Contains(Item_no_filter)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(Item_name_filter))
            {   //搜尋物料名稱
                ViewModel.ItemList = ViewModel.ItemList.Where(f => f.Item_name.Contains(Item_name_filter)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(Item_description_filter))
            {   //搜尋料品說明
                ViewModel.ItemList = ViewModel.ItemList.Where(f => f.Item_description.Contains(Item_description_filter)).ToList();
            }

            ViewModel.ItemListInPaging = PagingList.Create(ViewModel.ItemList, 20, (int)page, sort, "Item_no");
            ViewModel.ItemListInPaging.Action = "GetAllItem";

            ViewModel.ItemListInPaging.RouteValue = new RouteValueDictionary {
                    { "Item_no_filter", Item_no_filter},
                    { "Item_name_filter", Item_name_filter},
                    { "Item_description_filter", Item_description_filter}
            };
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public IActionResult GetItem_no(string term)
        {   //Item_no autocomplete
            try
            {
                Model.GetAllItem();
                if (term == null) term = "";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Json((from N in Model.ItemList
                         where N.Item_no.Contains(term)
                         select new { N.Item_no })
                        );
        }

        [HttpGet]
        [Route("[controller]/[action]/{no}")]
        public IActionResult GetItemByNo(string no)
        {   //Get Item
            SQLClass.Models.Item.Item temp = new Item();

            try
            {
                temp = Model.GetItem(no);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }            

            return new ObjectResult(new SQLClass.Models.Item.Item 
            { 
                Item_id = temp.Item_id,
                Item_no = temp.Item_no, 
                Item_name = temp.Item_name, 
                Item_description = temp.Item_description,
                Item_type = temp.Item_type,
                Reserved_field01 = temp.Reserved_field01
            });
        }

        public async Task<IActionResult> ItemSort(string Item_no_filter, string Item_name_filter, string Item_description_filter, int? page = 1, string sort = "Item_no")
        {
            try
            {
                Model.GetAllItem();
                ViewModel = _mapper.Map<JobViewModel>(Model);

                ItemPaging(Item_no_filter, Item_name_filter, Item_description_filter, page, sort);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_SelectItemModalPartial", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{no}")]
        public IActionResult GetItem_UOM(string no)
        {   //Item_no autocomplete
            List<SQLClass.Models.ItemUOM.ItemUOM> temp = new List<ItemUOM>();
            try
            {
                temp = Model.GetItem_UOM(no);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return Json(temp);
        }

        ///////////////////////////////////////////////////////////for routing///////////////////////////////////////////////////////////
        public async Task<IActionResult> RouteSort(int job_id, int? page = 1, string sort = "Routing_no")
        {
            try
            {
                Model.GetRoutingByJob_id(job_id);
                Model.WorkViewModel.Job_id = job_id;
                ViewModel = _mapper.Map<JobViewModel>(Model);

                ViewModel.Routing_Sort_str = sort;

                //ViewModel.JobListInPaging = await PagingList.CreateAsync(ViewModel.JobList.AsQueryable().OrderBy(s => s.Job_no), 3, (int)page, sort, "-Job_no");
                ViewModel.RoutingListInPaging = PagingList.Create(ViewModel.RoutingList, 20, (int)page, sort, "Routing_no");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_RoutingListPartial", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{job_no}")]
        public async Task<IActionResult> CreateRouting(string job_no)
        {   //show create routing
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetCreateRouting(user, job_no);
                ViewModel = _mapper.Map<JobViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_CreateRoutingModalPartial", ViewModel));
        }

        [HttpPost]
        [ActionName("SaveCreateRouting")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateRouting(IFormCollection data)
        {   //create routing
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostCreateRouting(data);
                    Model.WorkViewModel.Job_id = Convert.ToInt32(data["RoutingViewModel.Job_id"]);
                    ViewModel = _mapper.Map<JobViewModel>(Model);

                    ViewModel.Routing_Sort_str = "Routing_no";

                    ////ViewModel.JobListInPaging = await PagingList.CreateAsync(ViewModel.JobList.AsQueryable().OrderBy(s => s.Job_no), 3, (int)page, sort, "-Job_no");
                    ViewModel.RoutingListInPaging = PagingList.Create(ViewModel.RoutingList, 20, 1, "Routing_no", "Routing_no");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }
            //return await Task.Run(() => RedirectToAction("RouteSort", new { job_id = data["RoutingViewModel.Job_id"] }));
            return await Task.Run(() => PartialView("_RoutingListPartial", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{routing_no}")]
        public async Task<IActionResult> EditRouting(string routing_no)
        {   //show edit routing
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetEditRouting(routing_no, user);
                ViewModel = _mapper.Map<JobViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_EditRoutingModalPartial", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{no}")]
        public async Task<IActionResult> EditRouting(string no, IFormCollection data)
        {   //edit routing
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostEditRouting(no, data);

                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    Model.GetEditJob(ViewModel.WorkViewModel.Job_no, user);
                    ViewModel = _mapper.Map<JobViewModel>(Model);

                    ViewModel.Routing_Sort_str = "Routing_no";
                    ViewModel.RoutingListInPaging = PagingList.Create(ViewModel.RoutingList, 20, 1, "Routing_no", "Routing_no");                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_RoutingListPartial", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{job_no}/{routing_no}")]
        public async Task<IActionResult> DeleteRouting(string job_no, string routing_no)
        {   //delete routing
            try
            {
                if (ModelState.IsValid)
                {
                    Model.DisableRouting(routing_no);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("EditJob", new { no = job_no }));
        }

        ///////////////////////////////////////////////////////////remote validation///////////////////////////////////////////////////////////
        [AcceptVerbs("GET", "POST")]
        public IActionResult Customer_shipping_dateValidate(JobViewModel job)
        {
            int flag = DateTime.Compare(job.WorkViewModel.Customer_shipping_date.Value, DateTime.Today);

            if (flag < 0)
                return Json($"日期不可早於今日");
            else
                return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult Scheduled_completion_dateValidate(JobViewModel job)
        {
            int flag = DateTime.Compare(job.WorkViewModel.Scheduled_completion_date.Value, DateTime.Today);

            if (flag < 0)
                return Json($"日期不可早於今日");
            else
                return Json(true);
        }
    }
}
