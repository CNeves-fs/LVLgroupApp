using Core.Entities.Claims;
using Infrastructure.Data.DbContext;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultPrazosLimite
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedDefaultPrazosLimite(LVLgroupDbContext db)
        {
            //Seed Default Status

            if (db.Prazoslimite.Any()) return;


            var alarme_vermelho = new Prazolimite
            {
                Alarme = "Limite",
                LimiteMax = 1,
                LimiteMin = 0,
                Cortexto = "#ffffff",
                Corfundo = "#ff0000"
            };
            DbPrazolimiteSeed(alarme_vermelho, db);

            var alarme_laranja = new Prazolimite
            {
                Alarme = "Limite",
                LimiteMax = 4,
                LimiteMin = 2,
                Cortexto = "#ffffff",
                Corfundo = "#ff8000"
            };
            DbPrazolimiteSeed(alarme_laranja, db);

            var alarme_amarelo = new Prazolimite
            {
                Alarme = "Limite",
                LimiteMax = 8,
                LimiteMin = 5,
                Cortexto = "#151515",
                Corfundo = "#ffff00"
            };
            DbPrazolimiteSeed(alarme_amarelo, db);

            var alarme_verde = new Prazolimite
            {
                Alarme = "Limite",
                LimiteMax = 30,
                LimiteMin = 9,
                Cortexto = "#151515",
                Corfundo = "#00ff00"
            };
            DbPrazolimiteSeed(alarme_verde, db);
        }



        //---------------------------------------------------------------------------------------------------


        private static int DbPrazolimiteSeed(Prazolimite prazolimite, LVLgroupDbContext db)
        {
            db.Prazoslimite.Add(prazolimite);
            db.SaveChanges();
            return prazolimite.Id;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
