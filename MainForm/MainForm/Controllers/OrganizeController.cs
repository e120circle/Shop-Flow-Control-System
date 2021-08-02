using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MainForm.Areas.Identity.Data;
using MainForm.Models.Organize;
using MainForm.ViewModels.Organize;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLClass.Models.Company;

namespace MainForm.Controllers
{
    [Authorize]
    public class OrganizeController : Controller
    {
        public static OrganizeModel Model { get; set; }

        public static OrganizeViewModel ViewModel { get; set; }

        private readonly ILogger<OrganizeController> _logger;

        private readonly IMapper _mapper;

        private UserManager<MainFormUsers> _userManager { get; set; }

        public OrganizeController(ICompanyManage CompanyContext, ILogger<OrganizeController> logger, IMapper mapper, UserManager<MainFormUsers> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            Model = new OrganizeModel(CompanyContext, userManager);
            ViewModel = new OrganizeViewModel()
            {
                CompanyViewModel = new CompanyViewModel(),
                BusinessUnitViewModel = new BusinessUnitViewModel(),
                FactoryViewModel = new FactoryViewModel()
            };
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            try
            {
                Model.GetCompany();
                Model.GetAllBusinessUnit();
                if (Model.BusinessUnitList.Count != 0)
                    Model.SelectBusinessID = Model.BusinessUnitList.First().Business_unit_id.ToString();
                Model.GetFactoryList();          
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateCompany()
        {   //show create company
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetCreateCompany(user);
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpPost]
        [ActionName("SaveCreateCompany")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateCompany(IFormCollection data)
        {   //create company
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostCreateCompany(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", "Organize"));
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> EditCompany()
        {   //show edit company
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetEditCompany(user);
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View("EditCompany", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> EditCompany(string id, IFormCollection data)
        {   //edit company
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostEditCompany(id, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{company_id}")]
        public async Task<IActionResult> CreateBusinessUnit(string company_id)
        {   //show create business unit
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetCreateBusinessUnit(user, company_id);
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpPost]
        [ActionName("SaveCreateBusinessUnit")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateBusinessUnit(IFormCollection data)
        {   //create business unit
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostCreateBusinessUnit(data);
                    Model.GetAllBusinessUnit();
                }                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> EditBusinessUnit(string id)
        {   //show edit business unit
            try
            {
                Model.SelectBusinessID = id;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetEditBusinessUnit(id, user);
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> EditBusinessUnit(string id, IFormCollection data)
        {   //edit business unit
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostEditBusinessUnit(id, data);
                    Model.GetAllBusinessUnit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteBusinessUnit(string id)
        {   //delete business unit
            try
            {
                if (ModelState.IsValid)
                {
                    Model.DisableBusinessUnit(id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> SelectBusinessUnit(string id)
        {   //select business unit
            try
            {
                Model.SelectBusinessID = id;
                Model.GetFactoryList();
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => PartialView("_FactoryPartial", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{businessUnit_id}")]
        public async Task<IActionResult> CreateFactory(string businessUnit_id)
        {   //show create factory
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetCreateFactory(user, businessUnit_id);
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View("CreateFactory", ViewModel));
        }

        [HttpPost]
        [ActionName("SaveCreateFactory")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateFactory(IFormCollection data)
        {   //create factory
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostCreateFactory(data);
                    Model.SelectBusinessID = data["FactoryViewModel.Business_unit_id"];
                    Model.GetFactoryList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> EditFactory(string id)
        {   //show edit factory
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Model.GetEditFactory(id, user);
                ViewModel = _mapper.Map<OrganizeViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> EditFactory(string id, IFormCollection data)
        {   //edit factory
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostEditFactory(id, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index"));
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteFactory(string id)
        {   //delete factory
            try
            {
                if (ModelState.IsValid)
                {
                    Model.DisableFactory(id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("Index", ViewModel));
        }
    }
}
