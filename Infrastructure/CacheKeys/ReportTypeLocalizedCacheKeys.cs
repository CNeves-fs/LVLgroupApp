namespace Infrastructure.CacheKeys
{
    public static class ReportTypeLocalizedCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ReportTypeLocalizedList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int reportTypeLocalizedId) => $"ReportTypeLocalized-{reportTypeLocalizedId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNameKey(string name) => $"ReportTypeLocalizedName-{name}";


        //---------------------------------------------------------------------------------------------------


        public static string GetListLanguageKey(string language) => $"ListReportTypeLocalizedLanguage-{language}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromReportTypeKey(int reportTypeId) => $"ListReportTypeLocalizedFromReportTypeIdList-{reportTypeId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int reportTypeLocalizedId) => $"ReportTypeLocalizedDetails-{reportTypeLocalizedId}";


        //---------------------------------------------------------------------------------------------------

    }
}