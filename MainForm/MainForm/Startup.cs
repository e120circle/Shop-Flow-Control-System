using System;
using MainForm.Models.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQLClass.Models.Users;
using NLog;
using AutoMapper;
using MainForm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MainForm.Data;
using MainForm.Areas.Identity.Data;
using SQLClass.Models.Job;
using SQLClass.Models.Company;
using SQLClass.Models.Routing;
using SQLClass.Models.OperationsResource;
using SQLClass.Models.OperationsSubmit;
using SQLClass.Models.OperationsDetail;
using SQLClass.Models.SysCommon;
using SQLClass.Models.Item;
using SQLClass.Models.ItemUOM;
using SQLClass.Models.AutoNumber;
using ReflectionIT.Mvc.Paging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace MainForm
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Add(new ServiceDescriptor(typeof(MachineContext), new MachineContext(Configuration.GetConnectionString("Conn"))));
            services.AddSingleton<IUsersManage>(new UsersManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IJobManage>(new JobManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<ICompanyManage>(new CompanyManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IRoutingManage>(new RoutingManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IOperationsResourceManage>(new OperationsResourceManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IOperationsSubmitManage>(new OperationsSubmitManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IOperationsDetailManage>(new OperationsDetailManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<ISysCommonManage>(new SysCommonManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IItemManage>(new ItemManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IItemUOMManage>(new ItemUOMManageContext(Configuration.GetConnectionString("Conn")));
            services.AddSingleton<IAutoNumber>(new AutoNumberContext(Configuration.GetConnectionString("Conn")));
            services.AddDbContext<UsersContext>(options => options.UseMySQL(Configuration.GetConnectionString("UsersContextConnection")));

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
                    new CultureInfo("zh-TW"),
                    new CultureInfo("en-US")
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("zh-TW");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });        

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddPaging();
            // Register ViewComponent using an EmbeddedFileProvider & setting some options
            services.AddPaging(options => {
                options.ViewName = "pagination";
                options.HtmlIndicatorDown = " <span>&darr;</span>";
                options.HtmlIndicatorUp = " <span>&uarr;</span>";
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseStatusCodePagesWithRedirects("/Error");
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error/" + context.Response.StatusCode;
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseRequestLocalization();
            //app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            var supportedCultures = new string[] { "zh-TW", "en-US" };
            app.UseRequestLocalization(options =>
                options
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures)
                    .SetDefaultCulture("zh-TW")
            );
            app.UseCookiePolicy();
            LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("Conn");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{culture=zh-TW}/{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
