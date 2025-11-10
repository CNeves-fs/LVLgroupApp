namespace Infrastructure.CacheKeys
{
    public static class QuestionTemplateLocalizedCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "QuestionTemplateLocalizedList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int questionTemplateLocalizedId) => $"QuestionTemplateLocalized-{questionTemplateLocalizedId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetTextKey(string text) => $"QuestionTemplateLocalizedText-{text}";


        //---------------------------------------------------------------------------------------------------


        public static string GetLanguageKey(string language) => $"QuestionTemplateLocalizedLanguage-{language}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromQuestionTemplateKey(int questionTemplateId) => $"QuestionTemplateLocalizedFromQuestionTemplateIdList-{questionTemplateId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int questionTemplateLocalizedId) => $"QuestionTemplateLocalizedDetails-{questionTemplateLocalizedId}";


        //---------------------------------------------------------------------------------------------------

    }
}