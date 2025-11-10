using Core.Entities.Business;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultInfrastructure
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedInfrastructure(LVLgroupDbContext db, IWebHostEnvironment environment)
        {
            //Seed Empresas

            if (db.Empresas.Any()) return;

            var dirPath = Path.Combine(environment.WebRootPath, "images");
            var geoxPath = Path.Combine(dirPath, "logo-geox.png");
            var geoxArray = new byte[0];
            var skechersArray = new byte[0];

            var skechersPath = Path.Combine(dirPath, "logo-skechers.png");

            if (System.IO.File.Exists(geoxPath))
            {
                geoxArray = System.IO.File.ReadAllBytes(geoxPath);
            }
            if (System.IO.File.Exists(skechersPath))
            {
                skechersArray = System.IO.File.ReadAllBytes(skechersPath);
            }

            var geox = new Empresa
            {
                Nome = "GEOX",
                NomeCurto = "GEX",
                LogoPicture = geoxArray
            };
            var geoxId = DbEmpresaSeed(geox, db);

            var skechers = new Empresa
            {
                Nome = "SKECHERS",
                NomeCurto = "SKR",
                LogoPicture = skechersArray
            };
            var skechersId = DbEmpresaSeed(skechers, db);



            //Seed Agrupamentos de lojas

            var portugal = new Grupoloja
            {
                Nome = "Portugal",
                EmpresaId = geoxId,
                MaxDiasDecisão = 15
            };
            var gl = DbGrupolojaSeed(portugal, db);

        }


        //---------------------------------------------------------------------------------------------------


        private static int DbEmpresaSeed(Empresa empresa, LVLgroupDbContext db)
        {
            db.Empresas.Add(empresa);
            db.SaveChanges();
            return empresa.Id;
        }


        //---------------------------------------------------------------------------------------------------


        private static int DbGrupolojaSeed(Grupoloja grupoloja, LVLgroupDbContext db)
        {
            db.Gruposlojas.Add(grupoloja);
            db.SaveChanges();
            return grupoloja.Id;
        }


        //---------------------------------------------------------------------------------------------------


        private static int DbLojaSeed(Loja loja, LVLgroupDbContext db)
        {
            db.Lojas.Add(loja);
            db.SaveChanges();
            return loja.Id;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
