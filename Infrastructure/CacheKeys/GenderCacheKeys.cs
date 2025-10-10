namespace Infrastructure.CacheKeys
{
    public static class GenderCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "GendersList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "GendersSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int genderId) => $"Gender-{genderId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNomeKey(string nome) => $"Gender-{nome}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int genderId) => $"GenderDetails-{genderId}";


        //---------------------------------------------------------------------------------------------------

    }
}