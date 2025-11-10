namespace Infrastructure.CacheKeys
{
    public static class TipoOcorrenciaLocalizedCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "TipoOcorrenciaLocalizedList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int tipoOcorrenciaLocalizedId) => $"TipoOcorrenciaLocalized-{tipoOcorrenciaLocalizedId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNameKey(string name) => $"TipoOcorrenciaLocalizedName-{name}";


        //---------------------------------------------------------------------------------------------------


        public static string GetLanguageKey(string language) => $"TipoOcorrenciaLocalizedLanguage-{language}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTipoOcorrenciaKey(int tipoOcorrenciaId) => $"TiposOcorrenciasLocalizedFromTipoOcorrenciaIdList-{tipoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int tipoOcorrenciaLocalizedId) => $"TipoOcorrenciaLocalizedDetails-{tipoOcorrenciaLocalizedId}";


        //---------------------------------------------------------------------------------------------------

    }
}