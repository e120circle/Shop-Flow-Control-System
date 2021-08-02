using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MainForm.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SQLClass.Models.SysCommon;
using MainForm.Models.SysCommon;
using MainForm.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainForm.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MainFormUsers> _signInManager;
        private readonly UserManager<MainFormUsers> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private SysCommonModel SysCommonModel;

        public RegisterModel(
            UserManager<MainFormUsers> userManager,
            SignInManager<MainFormUsers> signInManager,
            ILogger<RegisterModel> logger,
            ISysCommonManage SysCommonContext)
        {
            _userManager ??= userManager;
            _signInManager ??= signInManager;
            _logger ??= logger;
            SysCommonModel ??= new SysCommonModel(SysCommonContext);

            RoleList = ShareModel.GetSelectList(SysCommonModel, "accout_role_type");
            if (RoleList[0].Text == "PMC") RoleList.RemoveAt(0);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "ChinessName")]
            public string ChinessName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Account_role_type")]
            public int Account_role_type { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "密碼長度需大於{2}個字元並小於{1}個字元", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "兩次輸入密碼不相同")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "is_enable")]
            public int Is_enable { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if(User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Home");
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new MainFormUsers { 
                    UserName = Input.Email,
                    Email = Input.Email, 
                    Name =  Input.Name,
                    ChinessName = Input.ChinessName,
                    Accout_role_type = Input.Account_role_type,
                    is_enable = 1
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
