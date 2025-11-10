using Core.Interfaces.Shared;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LVLgroupApp.Extensions
{
    public static class ApplicationBuilderExtension
    {

        //---------------------------------------------------------------------------------------------------


        public static void UseMultiLingualFeature(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
        }


        //---------------------------------------------------------------------------------------------------


        public static void UseSqlTableDependency(this IApplicationBuilder app, string connectionString)
        {
            //var serviceProvider = app.ApplicationServices;
            //var service = serviceProvider.GetRequiredService<SubscribeNotificationTableDependency>();
            //service?.SubscribeTableDependency(connectionString);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<SubscribeNotificationTableDependency>();
                service?.SubscribeTableDependency(connectionString);
            }


        }


        //---------------------------------------------------------------------------------------------------

    }
}