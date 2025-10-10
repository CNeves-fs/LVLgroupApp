using Core.Entities.Business;
using Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultMercados
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedMercados(LVLgroupDbContext db, IWebHostEnvironment environment)
        {

            //Seed mercados
            if (db.Mercados.Any()) return;

            var pt = new Mercado
            {
                Nome = "Portugal",
                NomeCurto = "PT"
            };
            DbMercadoSeed(pt, db);

            var es = new Mercado
            {
                Nome = "Espanha",
                NomeCurto = "ES"
            };
            DbMercadoSeed(es, db);

            var can = new Mercado
            {
                Nome = "Canárias",
                NomeCurto = "CAN"
            };
            DbMercadoSeed(can, db);

        }


        //---------------------------------------------------------------------------------------------------


        private static int DbMercadoSeed(Mercado mercado, LVLgroupDbContext db)
        {
            db.Mercados.Add(mercado);
            db.SaveChanges();
            return mercado.Id;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
