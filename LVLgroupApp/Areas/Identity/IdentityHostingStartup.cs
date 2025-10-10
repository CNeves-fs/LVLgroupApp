using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LVLgroupApp.Areas.Identity.IdentityHostingStartup))]
namespace LVLgroupApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {

        //---------------------------------------------------------------------------------------------------


        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }


        //---------------------------------------------------------------------------------------------------

    }
}