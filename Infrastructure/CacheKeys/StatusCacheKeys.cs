namespace Infrastructure.CacheKeys
{
    public static class StatusCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "StatusList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "StatusSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTypeKey(int statustype) => $"StatusFromType-{statustype}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int statusId) => $"Status-{statusId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int statusId) => $"StatusDetails-{statusId}";


        //---------------------------------------------------------------------------------------------------

    }
}