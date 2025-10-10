namespace Infrastructure.CacheKeys
{
    public static class ArtigoCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ArtigosList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "ArtigosSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"ArtigosFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromEmpresaKey(int empresaId) => $"ArtigosSelectListFromEmpresa-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int artigoId) => $"Artigo-{artigoId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int artigoId) => $"ArtigoDetails-{artigoId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetReferenciaKey(string referencia) => $"ArtigoDetails-{referencia}";


        //---------------------------------------------------------------------------------------------------

    }
}