namespace Infrastructure.CacheKeys
{
    public static class ReportTypeCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ReportTypesList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int reportTypeId) => $"ReportType-{reportTypeId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDefaultNameKey(string defaultName) => $"ReportTypeName-{defaultName}";

        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int reportTypeId) => $"ReportTypeDetails-{reportTypeId}";


        //---------------------------------------------------------------------------------------------------

    }
}