using Core.Entities.Cache;
using Core.Entities.Mail;
using Core.Interfaces.Shared;
using Core.Entities.Identity;
using Infrastructure.Data.DbContext;
using Infrastructure.Services;
using Infrastructure.Shared.Services;
using LVLgroupApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace LVLgroupApp.Extensions
{
    public static class ServiceCollectionExtensions
    {

        //---------------------------------------------------------------------------------------------------


        public static void AddMultiLingualSupport(this IServiceCollection services)
        {
            #region Registering ResourcesPath

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            #endregion Registering ResourcesPath

            services.AddMvc()
               .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
               .AddDataAnnotationsLocalization(options =>
               {
                   options.DataAnnotationLocalizerProvider = (type, factory) =>
                       factory.Create(typeof(SharedResource));
               });
            services.AddRouting(o => o.LowercaseUrls = true);
            services.AddHttpContextAccessor();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
                                    new CultureInfo("en"),
                                    new CultureInfo("pt"),
                                    new CultureInfo("es")
                };
                foreach(CultureInfo culture in cultures)
                {
                    culture.NumberFormat.CurrencySymbol = "€";
                    culture.NumberFormat.CurrencyDecimalSeparator = ".";
                    culture.NumberFormat.NumberDecimalSeparator = ".";
                }
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
        }


        //---------------------------------------------------------------------------------------------------


        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddPersistenceContexts(configuration);
            services.AddAuthenticationScheme(configuration);
        }


        //---------------------------------------------------------------------------------------------------


        private static void AddAuthenticationScheme(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(o =>
            {
                //Add Authentication to all Controllers by default.
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            });
        }


        //---------------------------------------------------------------------------------------------------


        private static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<LVLgroupDbContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                services.AddDbContext<LVLgroupDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("LVLgroupDbConnection")));
            }
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<LVLgroupDbContext>().AddDefaultUI().AddDefaultTokenProviders();
        }


        //---------------------------------------------------------------------------------------------------


        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            services.AddTransient<IMailService, SMTPMailService>();
            services.AddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
        }


        //---------------------------------------------------------------------------------------------------

    }
}