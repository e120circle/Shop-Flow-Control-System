using AutoMapper;
using MainForm.Models.OperationsResource;
using MainForm.ViewModels.OperationsResource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLClass.Models.OperationsResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Controllers
{
    [Authorize]
    public class OperationsResourceController : Controller
    {
        //public static RoutingResourceModel Model { get; set; }

        //public static RoutingResourceViewModel ViewModel { get; set; }

        //public IRoutingResourceManage _context { get; set; }

        //private readonly ILogger<ResourceController> _logger;

        //private readonly IMapper _mapper;

        //public ResourceController(IRoutingResourceManage context, ILogger<ResourceController> logger, IMapper mapper)
        //{
        //    _context = context;
        //    _logger = logger;
        //    _mapper = mapper;
        //    Model = new RoutingResourceModel();
        //    ViewModel = new RoutingResourceViewModel();
        //}

        //[HttpGet]
        //[Route("[controller]/[action]")]
        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        Model.RoutingResourceList = await _context.GetAllRoutingResource();
        //        ViewModel = _mapper.Map<RoutingResourceViewModel>(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //    }

        //    return await Task.Run(() => View(ViewModel));
        //}

        //[HttpGet]
        //[Route("[controller]/[action]")]
        //public async Task<IActionResult> CreateRoutingResource()
        //{   //show create routing resource
        //    try
        //    {
        //        ;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //    }

        //    return await Task.Run(() => View());
        //}

        //[HttpPost]
        //[Route("[controller]/[action]")]
        //public async Task<IActionResult> CreateRoutingResource([Bind("Routing_resource_id, Routing_id, Workstation_resource_id, " +
        //    "Routing_resource_type, Routing_resource_no, Routing_resource_name, Routing_resource_description, Mapping_sys_user, " +
        //    "Is_enable, Create_by, Create_date, Last_update_by, Last_update_date")] RoutingResource routingResource)
        //{   //create routing resource
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.CreateRoutingResource(routingResource);
        //        }

        //        Model.RoutingResourceList = await _context.GetAllRoutingResource();
        //        ViewModel = _mapper.Map<RoutingResourceViewModel>(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //    }

        //    return await Task.Run(() => RedirectToAction("Index", ViewModel));
        //}

        //[HttpGet]
        //[Route("[controller]/[action]/{id}")]
        //public async Task<IActionResult> EditRoutingResource(string id)
        //{   //show edit routing resource
        //    try
        //    {
        //        var temp = await _context.GetRoutingResource(id);
        //        Model.Routing_resource_id = temp.Routing_resource_id;
        //        Model.Routing_id = temp.Routing_id;
        //        Model.Workstation_resource_id = temp.Workstation_resource_id;
        //        Model.Routing_resource_type = temp.Routing_resource_type;
        //        Model.Routing_resource_no = temp.Routing_resource_no;
        //        Model.Routing_resource_name = temp.Routing_resource_name;
        //        Model.Routing_resource_description = temp.Routing_resource_description;
        //        Model.Mapping_sys_user = temp.Mapping_sys_user;
        //        Model.Is_enable = temp.Is_enable;
        //        Model.Create_by = temp.Create_by;
        //        Model.Create_date = temp.Create_date;
        //        Model.Last_update_by = temp.Last_update_by;
        //        Model.Last_update_date = temp.Last_update_date;

        //        ViewModel = _mapper.Map<RoutingResourceViewModel>(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //    }

        //    return await Task.Run(() => View("EditRoutingResource", ViewModel));
        //}

        //[HttpPost]
        //[Route("[controller]/[action]/{id}")]
        //public async Task<IActionResult> EditRoutingResource(string id, [Bind("Routing_resource_id, Routing_id, Workstation_resource_id, " +
        //    "Routing_resource_type, Routing_resource_no, Routing_resource_name, Routing_resource_description, Mapping_sys_user, " +
        //    "Is_enable, Create_by, Create_date, Last_update_by, Last_update_date")] RoutingResource routingResource)
        //{   //edit routing resource
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.EditRoutingResource(id, routingResource);
        //        }

        //        var temp = await _context.GetRoutingResource(id);
        //        Model.Routing_resource_id = temp.Routing_resource_id;
        //        Model.Routing_id = temp.Routing_id;
        //        Model.Workstation_resource_id = temp.Workstation_resource_id;
        //        Model.Routing_resource_type = temp.Routing_resource_type;
        //        Model.Routing_resource_no = temp.Routing_resource_no;
        //        Model.Routing_resource_name = temp.Routing_resource_name;
        //        Model.Routing_resource_description = temp.Routing_resource_description;
        //        Model.Mapping_sys_user = temp.Mapping_sys_user;
        //        Model.Is_enable = temp.Is_enable;
        //        Model.Create_by = temp.Create_by;
        //        Model.Create_date = temp.Create_date;
        //        Model.Last_update_by = temp.Last_update_by;
        //        Model.Last_update_date = temp.Last_update_date;

        //        ViewModel = _mapper.Map<RoutingResourceViewModel>(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //    }

        //    return await Task.Run(() => RedirectToAction("Index", ViewModel));
        //}
    }
}
