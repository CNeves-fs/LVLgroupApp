using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class TestSuperAdminUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Extra superadmin User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin2",
                Email = "superadmin2@gmail.com",
                FirstName = "superadmin2",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Colaborador.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Roles.GerenteLoja.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Revisor.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Supervisor.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}