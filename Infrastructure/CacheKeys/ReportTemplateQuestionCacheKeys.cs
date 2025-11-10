namespace Infrastructure.CacheKeys
{
    public static class ReportTemplateQuestionCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string AllReportTemplateQuestionListKey => "AllReportTemplateQuestionList";


        //---------------------------------------------------------------------------------------------------


        public static string AllActiveReportTemplateQuestionListKey => "AllActiveReportTemplateQuestionList";


        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ReportTemplateQuestionList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "ReportTemplateQuestionSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int reportTemplateQuestionId) => $"ReportTemplateQuestion-{reportTemplateQuestionId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListReportTemplateQuestionByQuestionTemplateIdKey(int questionTemplateId) => $"ListReportTemplateQuestionByQuestionTemplate-{questionTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListReportTemplateQuestionByReportTemplateIdKey(int reportTemplateId) => $"ListReportTemplateQuestionByReportTemplate-{reportTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int reportTemplateQuestionId) => $"ReportTemplateQuestionDetails-{reportTemplateQuestionId}";


        //---------------------------------------------------------------------------------------------------

    }
}