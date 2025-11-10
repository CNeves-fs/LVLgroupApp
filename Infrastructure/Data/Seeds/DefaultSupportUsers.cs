using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultSupportUsers
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default Support SuperAdmin User
            var supportUser = new ApplicationUser
            {
                UserName = "support",
                Email = "support@gmail.com",
                FirstName = "Support",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportUser, Roles.SuperAdmin.ToString());
                }
            }

            //Seed Default Support Admin User
            var supportAdmintUser = new ApplicationUser
            {
                UserName = "supportadmin",
                Email = "supportadmin@gmail.com",
                FirstName = "Supportadmin",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportAdmintUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportAdmintUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportAdmintUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportAdmintUser, Roles.Admin.ToString());
                }
            }

            //Seed Default Support Revisor User
            var supportRevisorUser = new ApplicationUser
            {
                UserName = "supportrevisor",
                Email = "supportrevisor@gmail.com",
                FirstName = "Supportrevisor",
                LastName = "Revisor",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportRevisorUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportRevisorUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportRevisorUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportRevisorUser, Roles.Revisor.ToString());
                }
            }

            //Seed Default Support Supervisor User
            var supportSupervisorUser = new ApplicationUser
            {
                UserName = "supportsupervisor",
                Email = "supportsupervisor@gmail.com",
                FirstName = "Supportsupervisor",
                LastName = "Supervisor",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportSupervisorUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportSupervisorUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportSupervisorUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportSupervisorUser, Roles.Supervisor.ToString());
                }
            }

            //Seed Default Support GerenteLoja User
            var supportGerenteLojaUser = new ApplicationUser
            {
                UserName = "supportgerenteloja",
                Email = "supportgerenteloja@gmail.com",
                FirstName = "Supportgerenteloja",
                LastName = "GerenteLoja",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportGerenteLojaUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportGerenteLojaUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportGerenteLojaUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportGerenteLojaUser, Roles.GerenteLoja.ToString());
                }
            }

            //Seed Default Support Colaborador User
            var supportColaboradorUser = new ApplicationUser
            {
                UserName = "supportcolaborador",
                Email = "supportcolaborador@gmail.com",
                FirstName = "Supportcolaborador",
                LastName = "Colaborador",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportColaboradorUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportColaboradorUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportColaboradorUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportColaboradorUser, Roles.Colaborador.ToString());
                }
            }

            //Seed Default Support Basic User
            var supportBasicUser = new ApplicationUser
            {
                UserName = "supportbasic",
                Email = "supportbasic@gmail.com",
                FirstName = "Supportbasic",
                LastName = "Basic",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            if (userManager.Users.All(u => u.Id != supportBasicUser.Id))
            {
                var user = await userManager.FindByEmailAsync(supportBasicUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(supportBasicUser, "@utoGrafica2023");
                    await userManager.AddToRoleAsync(supportBasicUser, Roles.Basic.ToString());
                }
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}