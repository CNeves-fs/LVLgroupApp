using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class TestGerenteLojaUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed default GerenteLoja Users

            var user1 = new ApplicationUser
            {
                UserName = "gerenteloja1",
                Email = "gerenteloja1@gmail.com",
                FirstName = "gerenteloja1",
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
                    await userManager.AddToRoleAsync(user1, Roles.GerenteLoja.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user2 = new ApplicationUser
            {
                UserName = "gerenteloja2",
                Email = "gerenteloja2@gmail.com",
                FirstName = "gerenteloja2",
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
                    await userManager.AddToRoleAsync(user2, Roles.GerenteLoja.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user3 = new ApplicationUser
            {
                UserName = "gerenteloja3",
                Email = "gerenteloja3@gmail.com",
                FirstName = "gerenteloja3",
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
                    await userManager.AddToRoleAsync(user3, Roles.GerenteLoja.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user4 = new ApplicationUser
            {
                UserName = "gerenteloja4",
                Email = "gerenteloja4@gmail.com",
                FirstName = "gerenteloja4",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != user4.Id))
            {
                var user = await userManager.FindByEmailAsync(user1.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user4, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(user4, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(user4, Roles.Colaborador.ToString());
                    await userManager.AddToRoleAsync(user4, Roles.GerenteLoja.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user5 = new ApplicationUser
            {
                UserName = "gerenteloja5",
                Email = "gerenteloja5@gmail.com",
                FirstName = "gerenteloja5",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != user5.Id))
            {
                var user = await userManager.FindByEmailAsync(user5.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user5, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(user5, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(user5, Roles.Colaborador.ToString());
                    await userManager.AddToRoleAsync(user5, Roles.GerenteLoja.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user6 = new ApplicationUser
            {
                UserName = "gerenteloja6",
                Email = "gerenteloja6@gmail.com",
                FirstName = "gerenteloja6",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != user6.Id))
            {
                var user = await userManager.FindByEmailAsync(user6.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(user6, "123Pa$$word!");
                    //await userManager.AddToRoleAsync(user6, Roles.Basic.ToString());
                    //await userManager.AddToRoleAsync(user6, Roles.Colaborador.ToString());
                    await userManager.AddToRoleAsync(user6, Roles.GerenteLoja.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}