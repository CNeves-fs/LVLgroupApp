namespace Infrastructure.CacheKeys
{
    public static class OcorrenciaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "OcorrenciasList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromCategoriaKey(int categoriaId) => $"OcorrenciasFromCategoriaList-{categoriaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTipoOcorrenciaKey(int tipoOcorrenciaId) => $"OcorrenciasFromTipoOcorrenciaList-{tipoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromStatusKey(int statusId) => $"OcorrenciasFromStatusList-{statusId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaKey(int lojaId) => $"OcorrenciasFromLojaList-{lojaId}";



        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoKey(int mercadoId) => $"OcorrenciasFromMercadoList-{mercadoId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"OcorrenciasFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaKey(int grupolojaId) => $"OcorrenciasFromGrupolojaList-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int ocorrenciaId) => $"Ocorrencia-{ocorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int ocorrenciaId) => $"OcorrenciaDetails-{ocorrenciaId}";


        //---------------------------------------------------------------------------------------------------

    }
}