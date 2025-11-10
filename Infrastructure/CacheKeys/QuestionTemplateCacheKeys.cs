namespace Infrastructure.CacheKeys
{
    public static class QuestionTemplateCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string AllQuestionTemplateListKey => "AllQuestionTemplateList";


        //---------------------------------------------------------------------------------------------------


        public static string AllActiveQuestionTemplateListKey => "AllActiveQuestionTemplateList";


        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "QuestionTemplateList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "QuestionTemplateSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTypeKey(int questionTypeId) => $"QuestionTemplateFromTypeList-{questionTypeId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromTypeKey(int questionTypeId) => $"QuestionTemplateSelectListFromType-{questionTypeId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int questionTemplateId) => $"QuestionTemplate-{questionTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int questionTemplateId) => $"QuestionTemplateDetails-{questionTemplateId}";


        //---------------------------------------------------------------------------------------------------

    }
}