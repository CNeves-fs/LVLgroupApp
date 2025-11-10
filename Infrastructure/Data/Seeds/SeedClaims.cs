using Core.Constants;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Data.Seeds
{
    public static class SeedClaims
    {

        //---------------------------------------------------------------------------------------------------


        public static async Task AddFullPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GenerateAllPermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task AddReadWritePermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GenerateEditPermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task AddViewOnlyPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GenerateViewPermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        public async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            await roleManager.AddFullPermissionClaim(adminRole, "AuditLogs");
            await roleManager.AddFullPermissionClaim(adminRole, "Users");

            await roleManager.AddFullPermissionClaim(adminRole, "Empresas");
            await roleManager.AddFullPermissionClaim(adminRole, "Gruposlojas");
            await roleManager.AddFullPermissionClaim(adminRole, "Lojas");
            await roleManager.AddFullPermissionClaim(adminRole, "Mercados");
            await roleManager.AddFullPermissionClaim(adminRole, "Vendas");
            await roleManager.AddFullPermissionClaim(adminRole, "Gruposupervisores");
            await roleManager.AddFullPermissionClaim(adminRole, "Lojagerentes");

            await roleManager.AddFullPermissionClaim(adminRole, "Artigos");
            await roleManager.AddFullPermissionClaim(adminRole, "Genders");

            await roleManager.AddFullPermissionClaim(adminRole, "Clientes");

            await roleManager.AddFullPermissionClaim(adminRole, "Claims");
            await roleManager.AddFullPermissionClaim(adminRole, "Fotos");
            await roleManager.AddFullPermissionClaim(adminRole, "Fototags");
            await roleManager.AddFullPermissionClaim(adminRole, "Statuss");
            await roleManager.AddFullPermissionClaim(adminRole, "Prazoslimite");

            await roleManager.AddFullPermissionClaim(adminRole, "Notifications");

            await roleManager.AddFullPermissionClaim(adminRole, "TiposOcorrencias");
            await roleManager.AddFullPermissionClaim(adminRole, "Ocorrencias");

            await roleManager.AddFullPermissionClaim(adminRole, "Vendas");

            await roleManager.AddFullPermissionClaim(adminRole, "QuestionTemplate");
            await roleManager.AddFullPermissionClaim(adminRole, "ReportTemplate");
            await roleManager.AddFullPermissionClaim(adminRole, "ReportType");
            await roleManager.AddFullPermissionClaim(adminRole, "Report");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Dashboards");
        }


        //---------------------------------------------------------------------------------------------------


        public async static Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Admin.ToString());


            await roleManager.AddViewOnlyPermissionClaim(adminRole, "AuditLogs");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Users");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Empresas");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Gruposlojas");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Lojas");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Mercados");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Vendas");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Gruposupervisores");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Lojagerentes");

            await roleManager.AddFullPermissionClaim(adminRole, "Artigos");
            await roleManager.AddFullPermissionClaim(adminRole, "Genders");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Clientes");

            await roleManager.AddFullPermissionClaim(adminRole, "Claims");
            await roleManager.AddFullPermissionClaim(adminRole, "Fotos");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Fototags");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Statuss");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Prazoslimite");

            await roleManager.AddFullPermissionClaim(adminRole, "Notifications");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "TiposOcorrencias");
            await roleManager.AddFullPermissionClaim(adminRole, "Ocorrencias");

            await roleManager.AddFullPermissionClaim(adminRole, "Vendas");

            await roleManager.AddFullPermissionClaim(adminRole, "QuestionTemplate");
            await roleManager.AddFullPermissionClaim(adminRole, "ReportTemplate");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "ReportType");
            await roleManager.AddFullPermissionClaim(adminRole, "Report");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Dashboards");
        }


        //---------------------------------------------------------------------------------------------------


        public async static Task SeedClaimsForSupervisor(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Supervisor.ToString());

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Users");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Lojas");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Vendas");
            await roleManager.AddReadWritePermissionClaim(adminRole, "Lojagerentes");

            await roleManager.AddFullPermissionClaim(adminRole, "Artigos");
            await roleManager.AddFullPermissionClaim(adminRole, "Genders");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Clientes");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Claims");
            await roleManager.AddFullPermissionClaim(adminRole, "Fotos");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Fototags");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Statuss");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Prazoslimite");

            await roleManager.AddFullPermissionClaim(adminRole, "Notifications");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "TiposOcorrencias");
            await roleManager.AddFullPermissionClaim(adminRole, "Ocorrencias");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Vendas");

            await roleManager.AddFullPermissionClaim(adminRole, "QuestionTemplate");
            await roleManager.AddFullPermissionClaim(adminRole, "ReportTemplate");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "ReportType");
            await roleManager.AddFullPermissionClaim(adminRole, "Report");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Dashboards");
        }


        //---------------------------------------------------------------------------------------------------


        public async static Task SeedClaimsForRevisor(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Revisor.ToString());

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Artigos");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Clientes");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Claims");
            await roleManager.AddReadWritePermissionClaim(adminRole, "Fotos");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Fototags");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Statuss");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Prazoslimite");

            await roleManager.AddFullPermissionClaim(adminRole, "Notifications");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "TiposOcorrencias");
            await roleManager.AddFullPermissionClaim(adminRole, "Ocorrencias");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Vendas");

            await roleManager.AddReadWritePermissionClaim(adminRole, "QuestionTemplate");
            await roleManager.AddReadWritePermissionClaim(adminRole, "ReportTemplate");
            await roleManager.AddReadWritePermissionClaim(adminRole, "ReportType");
            await roleManager.AddFullPermissionClaim(adminRole, "Report");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Dashboards");
        }


        //---------------------------------------------------------------------------------------------------


        public async static Task SeedClaimsForGerenteLoja(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.GerenteLoja.ToString());

            await roleManager.AddReadWritePermissionClaim(adminRole, "Artigos");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Clientes");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Claims");
            await roleManager.AddFullPermissionClaim(adminRole, "Fotos");

            await roleManager.AddFullPermissionClaim(adminRole, "Notifications");

            await roleManager.AddFullPermissionClaim(adminRole, "Ocorrencias");

            await roleManager.AddFullPermissionClaim(adminRole, "Vendas");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Report");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Dashboards");
        }


        //---------------------------------------------------------------------------------------------------


        public async static Task SeedClaimsForColaborador(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Colaborador.ToString());

            await roleManager.AddReadWritePermissionClaim(adminRole, "Artigos");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Clientes");

            await roleManager.AddReadWritePermissionClaim(adminRole, "Claims");
            await roleManager.AddFullPermissionClaim(adminRole, "Fotos");

            await roleManager.AddFullPermissionClaim(adminRole, "Notifications");

            await roleManager.AddFullPermissionClaim(adminRole, "Ocorrencias");

            await roleManager.AddFullPermissionClaim(adminRole, "Vendas");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Report");

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Dashboards");
        }


        public async static Task SeedClaimsForBasic(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Basic.ToString());

            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Claims");
            await roleManager.AddViewOnlyPermissionClaim(adminRole, "Ocorrencias");
        }


        //---------------------------------------------------------------------------------------------------

    }
}