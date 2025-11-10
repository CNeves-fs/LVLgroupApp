namespace Infrastructure.CacheKeys
{
    public static class ClaimCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ClaimsList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "ClaimsSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"ClaimsFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromEmpresaKey(int empresaId) => $"ClaimsSelectListFromEmpresa-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaKey(int grupolojaId) => $"ClaimsFromGrupolojaList-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromGrupolojaKey(int grupolojaId) => $"ClaimsSelectListFromGrupoloja-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaKey(int lojaId) => $"ClaimsFromLojaList-{lojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromLojaKey(int lojaId) => $"ClaimsSelectListFromLoja-{lojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int claimId) => $"Claim-{claimId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int claimId) => $"ClaimDetails-{claimId}";


        //---------------------------------------------------------------------------------------------------

    }
}