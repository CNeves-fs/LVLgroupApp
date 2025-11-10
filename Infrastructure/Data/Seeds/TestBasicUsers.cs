using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class TestBasicUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Basic User 1
            var basicUser1 = new ApplicationUser
            {
                UserName = "BasicUser1",
                Email = "basic1_lvlgroup@gmail.com",
                FirstName = "Basic1",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != basicUser1.Id))
            {
                var user = await userManager.FindByEmailAsync(basicUser1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(basicUser1, "123Pa$$word!");
                    await userManager.AddToRoleAsync(basicUser1, Roles.Basic.ToString());
                }
            }

            //Seed Basic User 2
            var basicUser2 = new ApplicationUser
            {
                UserName = "BasicUser2",
                Email = "basic2_lvlgroup@gmail.com",
                FirstName = "Basic2",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != basicUser2.Id))
            {
                var user = await userManager.FindByEmailAsync(basicUser2.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(basicUser2, "123Pa$$word!");
                    await userManager.AddToRoleAsync(basicUser2, Roles.Basic.ToString());
                }
            }

            //Seed Basic User 3
            var basicUser3 = new ApplicationUser
            {
                UserName = "BasicUser3",
                Email = "basic3_lvlgroup@gmail.com",
                FirstName = "Basic3",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != basicUser3.Id))
            {
                var user = await userManager.FindByEmailAsync(basicUser3.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(basicUser3, "123Pa$$word!");
                    await userManager.AddToRoleAsync(basicUser3, Roles.Basic.ToString());
                }
            }

            //Seed Basic User 4
            var basicUser4 = new ApplicationUser
            {
                UserName = "BasicUser4",
                Email = "basic4_lvlgroup@gmail.com",
                FirstName = "Basic4",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != basicUser4.Id))
            {
                var user = await userManager.FindByEmailAsync(basicUser4.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(basicUser4, "123Pa$$word!");
                    await userManager.AddToRoleAsync(basicUser4, Roles.Basic.ToString());
                }
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}