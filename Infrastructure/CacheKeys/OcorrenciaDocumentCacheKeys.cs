namespace Infrastructure.CacheKeys
{
    public static class OcorrenciaDocumentCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "OcorrenciaDocumentsList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromOcorrenciaKey(int? ocorrenciaId) => $"OcorrenciasDocumentsFromOcorrenciaList-{ocorrenciaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromFolderKey(string folder) => $"OcorrenciasDocumentsFromFolderList-{folder}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int ocorrenciaDocumentId) => $"OcorrenciaDocument-{ocorrenciaDocumentId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetFileNameKey(string fileName) => $"OcorrenciaDocumentFileName-{fileName}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int ocorrenciaDocumentId) => $"OcorrenciaDocumentDetails-{ocorrenciaDocumentId}";


        //---------------------------------------------------------------------------------------------------

    }
}