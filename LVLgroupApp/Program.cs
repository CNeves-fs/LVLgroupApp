using Infrastructure.Data.DbContext;
using Core.Entities.Identity;
using Infrastructure.Data.Seeds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace LVLgroupApp
{
    public class Program
    {

        //---------------------------------------------------------------------------------------------------


        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("LVLapp");

                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var dbContext = services.GetRequiredService<LVLgroupDbContext>();
                    var environment = services.GetRequiredService<IWebHostEnvironment>();

                    ClearHubConnections.ClearConnections(dbContext);
                    DefaultInfrastructure.SeedInfrastructure(dbContext, environment);
                    DefaultMercados.SeedMercados(dbContext, environment);
                    DefaultGenders.SeedDefaultGenders(dbContext);
                    DefaultFototags.SeedDefaultFototags(dbContext);
                    DefaultPrazosLimite.SeedDefaultPrazosLimite(dbContext);
                    DefaultStatus.SeedDefaultStatus(dbContext);

                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await SeedClaims.SeedClaimsForBasic(roleManager);
                    await SeedClaims.SeedClaimsForColaborador(roleManager);
                    await SeedClaims.SeedClaimsForGerenteLoja(roleManager);
                    await SeedClaims.SeedClaimsForRevisor(roleManager);
                    await SeedClaims.SeedClaimsForSupervisor(roleManager);
                    await SeedClaims.SeedClaimsForAdmin(roleManager);
                    await SeedClaims.SeedClaimsForSuperAdmin(roleManager);

                    await DefaultUsers.SeedAsync(userManager, roleManager);

                    //TestClientes.SeedClientes(dbContext, environment);

                    logger.LogInformation("Finished Seeding Default Data");
                    logger.LogInformation("Application Starting");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "An error occurred seeding the DB");
                }
            }
            host.Run();
        }


        //---------------------------------------------------------------------------------------------------


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        //---------------------------------------------------------------------------------------------------

    }
}
