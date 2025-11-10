namespace Infrastructure.CacheKeys
{
    public static class GrupolojaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "GruposlojasList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "GruposlojasSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"GruposlojasFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromEmpresaKey(int empresaId) => $"GruposlojasSelectListFromEmpresa-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int grupolojaId) => $"Grupoloja-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int grupolojaId) => $"GrupolojaDetails-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------

    }
}