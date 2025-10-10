namespace Infrastructure.CacheKeys
{
    public static class TipoOcorrenciaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "TipoOcorrenciasList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int tipoOcorrenciaId) => $"TipoOcorrencia-{tipoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDefaultNameKey(string defaultName) => $"TipoOcorrenciaName-{defaultName}";


        //---------------------------------------------------------------------------------------------------


        public static string GetCategoriaIdKey(int CategoriaId) => $"TipoOcorrenciaCategoria-{CategoriaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int tipoOcorrenciaId) => $"TipoOcorrenciaDetails-{tipoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------

    }
}