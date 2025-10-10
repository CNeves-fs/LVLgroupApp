using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultRoles
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Supervisor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Revisor.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.GerenteLoja.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Colaborador.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }


        //---------------------------------------------------------------------------------------------------

    }
}