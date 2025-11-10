namespace Infrastructure.CacheKeys
{
    public static class LojaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string AllLojasListKey => "AllLojasList";


        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "LojasList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "LojasSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaKey(int grupolojaId) => $"LojasFromGrupolojaList-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromGrupolojaKey(int grupolojaId) => $"LojasSelectListFromGrupoloja-{grupolojaId}";

        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"LojasFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromEmpresaKey(int empresaId) => $"LojasSelectListFromEmpresa-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoKey(int? mercadoId) => $"LojasFromMercadoList-{mercadoId}";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListFromMercadoKey(int? mercadoId) => $"LojasSelectListFromMercado-{mercadoId}";



        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int lojaId) => $"Loja-{lojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int lojaId) => $"LojaDetails-{lojaId}";


        //---------------------------------------------------------------------------------------------------

    }
}