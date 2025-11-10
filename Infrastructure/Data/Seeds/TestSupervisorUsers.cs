using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class TestSupervisorUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed default supervisor Users

            var user1 = new ApplicationUser
            {
                UserName = "supervisor1",
                Email = "supervisor1@gmail.com",
                FirstName = "supervisor1",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != user1.Id))
            {
                var user = await userManager.FindByEmailAsync(user1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user1, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(user1, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(user1, Roles.Colaborador.ToString());
                    //await userManager.AddToRoleAsync(user1, Roles.GerenteLoja.ToString());
                    //await userManager.AddToRoleAsync(user1, Roles.Revisor.ToString());
                    await userManager.AddToRoleAsync(user1, Roles.Supervisor.ToString());
                }
                await roleManager.SeedClaimsForSupervisor();
            }

            var user2 = new ApplicationUser
            {
                UserName = "supervisor2",
                Email = "supervisor2@gmail.com",
                FirstName = "supervisor2",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != user2.Id))
            {
                var user = await userManager.FindByEmailAsync(user2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user2, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(user2, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(user2, Roles.Colaborador.ToString());
                    //await userManager.AddToRoleAsync(user2, Roles.GerenteLoja.ToString());
                    //await userManager.AddToRoleAsync(user2, Roles.Revisor.ToString());
                    await userManager.AddToRoleAsync(user2, Roles.Supervisor.ToString());
                }
                await roleManager.SeedClaimsForSupervisor();
            }

            var user3 = new ApplicationUser
            {
                UserName = "supervisor3",
                Email = "supervisor3@gmail.com",
                FirstName = "supervisor3",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != user3.Id))
            {
                var user = await userManager.FindByEmailAsync(user3.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user3, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(user3, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(user3, Roles.Colaborador.ToString());
                    //await userManager.AddToRoleAsync(user3, Roles.GerenteLoja.ToString());
                    //await userManager.AddToRoleAsync(user3, Roles.Revisor.ToString());
                    await userManager.AddToRoleAsync(user3, Roles.Supervisor.ToString());
                }
                await roleManager.SeedClaimsForSupervisor();
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}