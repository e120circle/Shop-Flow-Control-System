using AutoMapper;
using MainForm.Areas.Identity.Data;
using MainForm.Models.Users;
using MainForm.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReflectionIT.Mvc.Paging;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public static UsersModel Model { get; set; }

        public static UsersViewModel ViewModel { get; set; }

        private readonly ILogger<UsersController> _logger;

        private readonly IMapper _mapper;

        public UsersController(IUsersManage UsersContext, ILogger<UsersController> logger, IMapper mapper,
            ISysCommonManage SysCommonContext, UserManager<MainFormUsers> userManager)
        {
            _logger ??= logger;
            _mapper ??= mapper;
            _userManager ??= userManager;

            Model ??= new UsersModel(UsersContext, SysCommonContext);
            ViewModel ??= new UsersViewModel();
            ViewModel.UserViewModel ??= new UserViewModel();
        }

        private UserManager<MainFormUsers> _userManager { get; set; }

        public async Task<IActionResult> Index( int? page = 1, string sort = "Name")
        {
            try
            {
                Model.GetAllUsers();

                ViewModel = _mapper.Map<UsersViewModel>(Model);

                ViewModel.Sort_str = sort;

                ViewModel.UsersListInPaging = PagingList.Create(ViewModel.UsersList, 10, (int)page, sort, "Name");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View(ViewModel));
        }

        [HttpGet]
        [Route("[controller]/[action]/{name}")]
        public async Task<IActionResult> EditUser(string name)
        {   //show edit users
            try
            {
                Model.GetEditUsers(name);
                ViewModel = _mapper.Map<UsersViewModel>(Model);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => View("EditUser", ViewModel));
        }

        [HttpPost]
        [Route("[controller]/[action]/{name}")]
        public async Task<IActionResult> EditUser(string name, IFormCollection data)
        {   //edit users
            try
            {
                if (ModelState.IsValid)
                {
                    Model.PostEditUsers(name, data);
                    Model.GetEditUsers(name);
                    ViewModel = _mapper.Map<UsersViewModel>(Model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
            }

            return await Task.Run(() => RedirectToAction("EditUser", new { name = data["UsersViewModel.Name"] }));
        }
    }
}
