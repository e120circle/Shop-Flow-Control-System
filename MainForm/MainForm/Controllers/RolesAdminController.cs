using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MainForm.Areas.Identity.Data;
using MainForm.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MainForm.Controllers
{
    public class RolesAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private UserManager<MainFormUsers> _userManager;

        public RolesAdminController(UserManager<MainFormUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var identityRoles = await _roleManager.Roles.ToListAsync();
            return View(identityRoles);
        }

        [Authorize (Roles = "PMC, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize (Roles = "PMC, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, NormalizedName")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);

                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        [Authorize(Roles = "PMC, Admin, Manager")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<MainFormUsers> members = new List<MainFormUsers>();
            List<MainFormUsers> nonMembers = new List<MainFormUsers>();

            foreach (MainFormUsers user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [Authorize(Roles = "PMC, Admin, Manager")]
        [HttpPost]
        [Route("[controller]/[action]/{model}")]
        public async Task<IActionResult> Update([FromForm]RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    MainFormUsers user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        //if (!result.Succeeded)
                        //    Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    MainFormUsers user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        //if (!result.Succeeded)
                        //    Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(model.RoleId);
        }
    }
}
