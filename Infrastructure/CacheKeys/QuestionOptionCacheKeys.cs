namespace Infrastructure.CacheKeys
{
    public static class QuestionOptionCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string AllQuestionOptionListKey => "AllQuestionOptionList";


        //---------------------------------------------------------------------------------------------------


        public static string AllActiveQuestionOptionListKey => "AllActiveQuestionOptionList";


        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "QuestionOptionList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "QuestionOptionSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromQuestionTemplateIdKey(int questionTemplateId) => $"QuestionOptionFromQuestionTemplateIdList-{questionTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromQuestionTemplateIdKey(int questionTemplateId) => $"QuestionOptionSelectListFromQuestionTemplateId-{questionTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int questionOptionId) => $"QuestionOption-{questionOptionId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int questionOptionId) => $"QuestionOptionDetails-{questionOptionId}";


        //---------------------------------------------------------------------------------------------------

    }
}