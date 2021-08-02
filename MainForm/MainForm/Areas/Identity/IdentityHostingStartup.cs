using System;
using MainForm.Areas.Identity.Data;
using MainForm.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(MainForm.Areas.Identity.IdentityHostingStartup))]
namespace MainForm.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UsersContext>(options =>
                    options.UseMySQL(
                        context.Configuration.GetConnectionString("UsersContextConnection")));

                services.AddDefaultIdentity<MainFormUsers>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddUserManager<UserManager<MainFormUsers>>();

                //services.AddDefaultIdentity<MainFormUsers>(options => options.SignIn.RequireConfirmedAccount = false)
                //    .AddEntityFrameworkStores<UsersContext>();
            });
        }
    }
}