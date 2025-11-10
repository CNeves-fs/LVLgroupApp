using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class TestColaboradorUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed default colaborador Users

            var user1 = new ApplicationUser
            {
                UserName = "colaborador1",
                Email = "colaborador1@gmail.com",
                FirstName = "Colaborador1",
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
                    await userManager.AddToRoleAsync(user1, Roles.Colaborador.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user2 = new ApplicationUser
            {
                UserName = "colaborador2",
                Email = "colaborador2@gmail.com",
                FirstName = "Colaborador2",
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
                    await userManager.AddToRoleAsync(user2, Roles.Colaborador.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user3 = new ApplicationUser
            {
                UserName = "colaborador3",
                Email = "colaborador3@gmail.com",
                FirstName = "Colaborador3",
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
                    await userManager.AddToRoleAsync(user3, Roles.Colaborador.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user4 = new ApplicationUser
            {
                UserName = "colaborador4",
                Email = "colaborador4@gmail.com",
                FirstName = "Colaborador4",
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
                    await userManager.AddToRoleAsync(user4, Roles.Colaborador.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user5 = new ApplicationUser
            {
                UserName = "colaborador5",
                Email = "colaborador5@gmail.com",
                FirstName = "Colaborador5",
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
                    await userManager.AddToRoleAsync(user5, Roles.Colaborador.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }

            var user6 = new ApplicationUser
            {
                UserName = "colaborador6",
                Email = "colaborador6@gmail.com",
                FirstName = "Colaborador6",
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
                    await userManager.AddToRoleAsync(user6, Roles.Colaborador.ToString());
                }
                await roleManager.SeedClaimsForColaborador();
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}