namespace Infrastructure.CacheKeys
{
    public static class ClienteCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "ClienteList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "ClienteSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int clienteId) => $"Cliente-{clienteId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetTelefoneKey(string telefone) => $"Cliente-{telefone}";


        //---------------------------------------------------------------------------------------------------


        public static string GetEmailKey(string email) => $"Cliente-{email}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNIFKey(string nif) => $"Cliente-{nif}";


        //---------------------------------------------------------------------------------------------------


        public static string GetNomeKey(string nome) => $"Cliente-{nome}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int clienteId) => $"ClienteDetails-{clienteId}";


        //---------------------------------------------------------------------------------------------------

    }
}