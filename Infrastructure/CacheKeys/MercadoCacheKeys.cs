namespace Infrastructure.CacheKeys
{
    public static class MercadoCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "MercadoList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "MercadoSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int mercadoId) => $"Mercado-{mercadoId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNomeKey(string nome) => $"Mercado-{nome}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int mercadoId) => $"MercadoDetails-{mercadoId}";


        //---------------------------------------------------------------------------------------------------

    }
}