using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainForm.Models;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using SQLClass.Models.Job;
using SQLClass.Models.Routing;
using AutoMapper;
using SQLClass.Models.ItemUOM;
using SQLClass.Models.Item;
using SQLClass.Models.SysCommon;
using MainForm.Models.Home;
using MainForm.ViewModels.Home;
using ReflectionIT.Mvc.Paging;
using MainForm.ViewModels.Job;
using Newtonsoft.Json;
using SQLClass.Models.OperationsSubmit;

namespace MainForm.Controllers
{
    public class KanbanController : Controller
    {
        public static HomeModel Model { get; set; }

        public static HomeViewModel ViewModel { get; set; }

        private readonly IMapper _mapper;

        private readonly ILogger<KanbanController> _logger;

        private JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public KanbanController(IJobManage JobContext, IRoutingManage RoutingContext, IOperationsSubmitManage SubmitContext, 
            ILogger<KanbanController> logger, ISysCommonManage SysCommonContext, IMapper mapper)
        {
            _logger ??= logger;
            _mapper ??= mapper;

            Model ??= new HomeModel(JobContext, RoutingContext, SubmitContext, SysCommonContext);
            ViewModel ??= new HomeViewModel();
        }

        [HttpGet]
        //[Route("[controller]/[action]/{page}")]
        public async Task<IActionResult> Index(IFormCollection data, int? page = 1, string sort = "-Job_no")
        {
            try
            {
                Model.GetAllJobByJobDate();
                List<List<SQLClass.Models.Routing.Routing>> Routing_list = new List<List<SQLClass.Models.Routing.Routing>>();

                ViewModel = _mapper.Map<HomeViewModel>(Model);
                ViewModel.Sort_str = sort;

                ViewModel.JobListInPaging = PagingList.Create(ViewModel.JobList, 100000, (int)page, sort, "-Job_no");

                Routing_list = new List<List<Routing>>();
                Model.SubmitList = new List<OperationsSubmit>();
                foreach (SQLClass.Models.Job.Job a in ViewModel.JobListInPaging)
                {
                    Model.GetRoutingByJob_no(a.Job_no);
                    Routing_list.Add(Model.RoutingList);
                }
                ViewModel.RoutingsList = Routing_list;
                ViewModel.SubmitList = Model.SubmitList;
                //List<DataPoint> _dataPoints_temp = new List<DataPoint>();
                //List<List<DataPoint>> _dataPoints = new List<List<DataPoint>>();

                //foreach (List<Routing> a in Routing_list)
                //{
                //    _dataPoints_temp = new List<DataPoint>();

                //    foreach (Routing b in a)
                //    {
                //        int y = b.Scheduled_completion_quantity;
                //        string label = "123";

                //        _dataPoints_temp.Add(new DataPoint(y, label));
                //    }

                //    _dataPoints.Add(_dataPoints_temp);
                //}

                //int y = Routing_list[0][0].Scheduled_completion_quantity;
                //string label = Routing_list[0][0].Routing_name;

                //_dataPoints_temp.Add(new DataPoint(y, label));
                //_dataPoints.Add(_dataPoints_temp);
                //ViewBag.DataPoints = JsonConvert.SerializeObject(_dataPoints_temp, _jsonSetting);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [Route("/[controller]/[action]/{StatusCode}")]
        public IActionResult Error(int code)
        {
            switch (code)
            {
                case 404:
                    ViewBag.ErrorMessasge = $"I am having {code}" + " Error code Message";
                    break;
            }

            return View();
        }

        [HttpPost]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
    }
}
