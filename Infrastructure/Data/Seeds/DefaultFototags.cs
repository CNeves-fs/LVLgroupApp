using Core.Entities.Claims;
using Infrastructure.Data.DbContext;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultFototags
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedDefaultFototags(LVLgroupDbContext db)
        {
            //Seed Default Fototags

            if (db.Fototags.Any()) return;

            var tagList = new List<Fototag>();
            tagList.Add(new Fototag { Tag = "PALMILHA DIR." });
            tagList.Add(new Fototag { Tag = "PALMILHA ESQ." });
            tagList.Add(new Fototag { Tag = "BIQUEIRA DIR." });
            tagList.Add(new Fototag { Tag = "BIQUEIRA ESQ." });
            tagList.Add(new Fototag { Tag = "CALCANHAR DIR." });
            tagList.Add(new Fototag { Tag = "CALCANHAR ESQ." });
            tagList.Add(new Fototag { Tag = "SOLA DIR." });
            tagList.Add(new Fototag { Tag = "SOLA ESQ." });
            tagList.Add(new Fototag { Tag = "VISTA LATERAL DIR." });
            tagList.Add(new Fototag { Tag = "VISTA LATERAL ESQ." });

            foreach(Fototag tag in tagList)
            {
                DbFototagSeed(tag, db);
            }
        }


        //---------------------------------------------------------------------------------------------------


        private static int DbFototagSeed(Fototag fototag, LVLgroupDbContext db)
        {
            db.Fototags.Add(fototag);
            db.SaveChanges();
            return fototag.Id;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
