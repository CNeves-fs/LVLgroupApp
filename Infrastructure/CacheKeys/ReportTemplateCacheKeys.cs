namespace Infrastructure.CacheKeys
{
    public static class ReportTemplateCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string AllReportTemplateListKey => "AllReportTemplateList";


        //---------------------------------------------------------------------------------------------------


        public static string AllActiveReportTemplateListKey => "AllActiveReportTemplateList";


        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ReportTemplateList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "ReportTemplateSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListIsActiveKey => $"ReportTemplateIsActiveList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int reportTemplateId) => $"ReportTemplate-{reportTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListReportTemplateByReportTypeIdKey(int reportTypeId) => $"ListReportTemplateByReportType-{reportTypeId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int reportTemplateId) => $"ReportTemplateDetails-{reportTemplateId}";


        //---------------------------------------------------------------------------------------------------

    }
}