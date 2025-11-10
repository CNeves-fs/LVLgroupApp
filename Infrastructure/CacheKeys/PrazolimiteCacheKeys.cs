namespace Infrastructure.CacheKeys
{
    public static class PrazolimiteCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "PrazolimiteList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "PrazolimiteSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int prazolimiteId) => $"Prazolimite-{prazolimiteId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int prazolimiteId) => $"PrazolimiteDetails-{prazolimiteId}";


        //---------------------------------------------------------------------------------------------------

    }
}