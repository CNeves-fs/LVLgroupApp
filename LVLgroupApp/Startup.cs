using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Core.Extensions;
using DataTables.AspNet.AspNetCore;
using FluentValidation;
using Infrastructure.Extensions;
using Infrastructure.Hubs;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Extensions;
using LVLgroupApp.Hubs;
using LVLgroupApp.Permission;
using LVLgroupApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PdfSharp.Fonts;
using Serilog;
using System;
using System.Reflection;

namespace LVLgroupApp
{
    public class Startup
    {

        //---------------------------------------------------------------------------------------------------


        public IConfiguration _configuration { get; }


        //---------------------------------------------------------------------------------------------------


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
        }


        //---------------------------------------------------------------------------------------------------


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            GlobalFontSettings.UseWindowsFontsUnderWindows = true;

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddNotyf(o =>
            {
                o.DurationInSeconds = 10;
                o.IsDismissable = true;
                o.HasRippleEffect = true;
                o.Position = NotyfPosition.BottomRight;
            });

            services.AddSignalR();

            services.AddCoreLayer();
            services.AddInfrastructure(_configuration);
            services.AddPersistenceContexts(_configuration);
            services.AddMailService();
            services.AddRepositories();
            services.AddSharedInfrastructure(_configuration);
            services.AddMultiLingualSupport();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60*20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            //services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IViewRenderService, ViewRenderService>();
            services.RegisterDataTables();

            services.AddMvc().AddViewOptions(options => 
            { 
                options.HtmlHelperOptions.ClientValidationEnabled = true; 
            });




        }


        //---------------------------------------------------------------------------------------------------


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Home/Error?statusCode=500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            Newtonsoft.Json.JsonConvert.DefaultSettings = () => new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            //app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseStatusCodePagesWithReExecute("/Home/Home/Error", "?statusCode={0}");

            app.UseNotyf();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMultiLingualFeature();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<ProgressHub>("/progressHub");
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseSqlTableDependency(_configuration.GetConnectionString("LVLgroupDbConnection"));
        }


        //---------------------------------------------------------------------------------------------------

    }
}
