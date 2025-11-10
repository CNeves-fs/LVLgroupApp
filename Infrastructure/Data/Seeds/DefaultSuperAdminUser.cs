using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultSuperAdminUser
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default SuperAdmin User
            var superadminUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != superadminUser.Id))
            {
                var user = await userManager.FindByEmailAsync(superadminUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(superadminUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(superadminUser, Roles.SuperAdmin.ToString());
                }
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}