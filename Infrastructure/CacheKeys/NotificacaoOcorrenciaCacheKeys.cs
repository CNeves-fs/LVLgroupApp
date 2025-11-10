namespace Infrastructure.CacheKeys
{
    public static class NotificacaoOcorrenciaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "NotificacoesOcorrenciasList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTipoOcorrenciaKey(int tipoOcorrenciaId) => $"NotificacoesOcorrenciasFromTipoOcorrenciaList-{tipoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int notificacaoOcorrenciaId) => $"NotificacaoOcorrencia-{notificacaoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int notificacaoOcorrenciaId) => $"NotificacaoOcorrenciaDetails-{notificacaoOcorrenciaId}";


        //---------------------------------------------------------------------------------------------------

    }
}