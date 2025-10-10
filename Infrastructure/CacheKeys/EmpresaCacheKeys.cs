namespace Infrastructure.CacheKeys
{
    public static class EmpresaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "EmpresaList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "EmpresaSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int empresaId) => $"Empresa-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNomeKey(string nome) => $"Empresa-{nome}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int empresaId) => $"EmpresaDetails-{empresaId}";


        //---------------------------------------------------------------------------------------------------

    }
}