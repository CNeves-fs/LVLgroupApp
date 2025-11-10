namespace Infrastructure.CacheKeys
{
    public static class ReportCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string AllReportListKey => "AllReportList";


        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ReportList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "ReportSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int reportId) => $"Report-{reportId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListReportByLojaIdKey(int lojaId) => $"ListReportByLoja-{lojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListReportByReportTemplateIdKey(int reportTemplateId) => $"ListReportByReportTemplate-{reportTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int reportId) => $"ReportDetails-{reportId}";


        //---------------------------------------------------------------------------------------------------

    }
}