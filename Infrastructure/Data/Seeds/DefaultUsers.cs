using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (userManager.Users.Any()) return;
            await DefaultSuperAdminUser.SeedAsync(userManager, roleManager);
            await DefaultSupportUsers.SeedAsync(userManager, roleManager);

            //await TestSuperAdminUsers.SeedAsync(userManager, roleManager);
            //await TestAdminUsers.SeedAsync(userManager, roleManager);
            //await TestSupervisorUsers.SeedAsync(userManager, roleManager);
            //await TestRevisorUsers.SeedAsync(userManager, roleManager);
            //await TestGerenteLojaUsers.SeedAsync(userManager, roleManager);
            //await TestColaboradorUsers.SeedAsync(userManager, roleManager);
            //await TestBasicUsers.SeedAsync(userManager, roleManager);
        }


        //---------------------------------------------------------------------------------------------------

    }
}