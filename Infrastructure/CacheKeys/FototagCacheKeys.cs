namespace Infrastructure.CacheKeys
{
    public static class FototagCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "FototagsList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "FototagsSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int fototagId) => $"Fototag-{fototagId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int fototagId) => $"FototagDetails-{fototagId}";


        //---------------------------------------------------------------------------------------------------

    }
}