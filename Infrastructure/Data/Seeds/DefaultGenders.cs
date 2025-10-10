
using Core.Entities.Artigos;
using Infrastructure.Data.DbContext;
using System.Linq;

namespace Infrastructure.Data.Seeds
{
    public static class DefaultGenders
    {

        //---------------------------------------------------------------------------------------------------


        public static void SeedDefaultGenders(LVLgroupDbContext db)
        {
            //Seed Default Genders

            if (db.Genders.Any()) return;

            var baby = new Gender
            {
                Nome = "BABY",
                TamanhosNum = "18,5; 19; 19,5; 20; 20,5; 21; 21,5; 22; 22,5; 23; 23,5; 24; 24,5; 25; 25,5; 26; 26,5; 27",
                TamanhosAlf = ""
            };
            DbGenderSeed(baby, db);

            var junior = new Gender
            {
                Nome = "JÚNIOR",
                TamanhosNum = "24; 24,5; 25; 25,5; 26; 26,5; 27; 27,5; 28; 28,5; 29; 29,5; 30; 30,5; 31; 31,5; 32; 32,5; 33; 33,5; 34; 34,5; 35; 35,5; 36; 36,5; 37; 37,5; 38; 38,5; 39; 39,5; 40; 40,5; 41",
                TamanhosAlf = "XS; S"
            };
            DbGenderSeed(junior, db);

            var women = new Gender
            {
                Nome = "WOMENS",
                TamanhosNum = "35; 35,5; 36; 36,5; 37; 37,5; 38; 38,5; 39; 39,5; 40; 40,5; 41; 41,5; 42",
                TamanhosAlf = "XS; S; M; L; X; XL; XXL; XXXL"
            };
            DbGenderSeed(women, db);

            var men = new Gender
            {
                Nome = "MENS",
                TamanhosNum = "39; 39,5; 40; 40,5; 41; 41,5; 42; 42,5; 43; 43,5; 44; 44,5; 45; 45,5; 46; 46,5; 47; 47,5",
                TamanhosAlf = "XS; S; M; L; X; XL; XXL; XXXL"
            };
            DbGenderSeed(men, db);

            var nogender = new Gender
            {
                Nome = "NO GENDER",
                TamanhosNum = "18,5; 19; 19,5; 20; 20,5; 21; 21,5; 22; 22,5; 23; 23,5; 24; 24,5; 25; 25,5; 26; 26,5; 27; 27,5; 28; 28,5; 29; 29,5; 30; 30,5; 31; 31,5; 32; 32,5; 33; 33,5; 34; 34,5; 35; 35,5; 36; 36,5; 37; 37,5; 38; 38,5; 39; 39,5; 40; 40,5; 41; 41,5; 42; 42,5; 43; 43,5; 44; 44,5; 45; 45,5; 46; 46,5; 47; 47,5",
                TamanhosAlf = "XS; S; M; L; X; XL"
            };
            DbGenderSeed(nogender, db);
        }


        //---------------------------------------------------------------------------------------------------


        private static int DbGenderSeed(Gender gender, LVLgroupDbContext db)
        {
            db.Genders.Add(gender);
            db.SaveChanges();
            return gender.Id;
        }


        //---------------------------------------------------------------------------------------------------

    }
}
