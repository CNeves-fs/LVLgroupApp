namespace Infrastructure.CacheKeys
{
    public static class VendaDiariaCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "VendasDiariasList";


        //---------------------------------------------------------------------------------------------------


        public static string SelectListKey => "VendasDiariasSelectList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoKey(int mercadoId) => $"VendasDiariasFromMercadoList-{mercadoId}";



        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoAnoKey(int mercadoId, int ano) => $"VendasDiariasFromMercadoAnoList-{mercadoId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoMesKey(int mercadoId, int ano, int mes) => $"VendasDiariasFromMercadoMesList-{mercadoId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"VendasDiariasFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaAnoKey(int empresaId, int ano) => $"VendasDiariasFromEmpresaAnoList-{empresaId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaMesKey(int empresaId, int ano, int mes) => $"VendasDiariasFromEmpresaMesList-{empresaId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaKey(int grupolojaId) => $"VendasDiariasFromGrupolojaList-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaAnoKey(int grupolojaId, int ano) => $"VendasDiariasFromGrupolojaAnoList-{grupolojaId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaMesKey(int grupolojaId, int ano, int mes) => $"VendasDiariasFromGrupolojaMesList-{grupolojaId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaKey(int lojaId) => $"VendasDiariasFromLojaList-{lojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaAnoKey(int lojaId, int ano) => $"VendasDiariasFromLojaAno-{lojaId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaMesKey(int lojaId, int ano, int mês) => $"VendasDiariasFromLojaMes-{lojaId}-{ano}-{mês}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKeyFromLojaData(int lojaId, int ano, int mês, int diaDoMês) => $"VendaDiariaFromLojaData-{lojaId}-{ano}-{mês}-{diaDoMês}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromAnoKey(int ano) => $"VendasDiariasFromAnoList-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromTrimestreKey(int ano, int trimestre) => $"VendasDiariasFromTrimestreList-{ano}-{trimestre}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMesKey(int ano, int mês) => $"VendasDiariasFromMesList-{ano}-{mês}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromSemanaKey(int ano, int semana) => $"VendasDiariasFromSemanaList-{ano}-{semana}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromDiaKey(int ano, int mês, int dia) => $"VendasDiariasFromDiaList-{ano}-{mês}-{dia}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromVendaSemanalKey(int vendaSemanalId) => $"VendasDiariasFromVendaSemanalList-{vendaSemanalId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKeyFromVendaSemanalDia(int vendaSemanalId, int diaDaSemana) => $"VendaDiariaFromVendaSemanalDia-{vendaSemanalId}-{diaDaSemana}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int vendaDiariaId) => $"VendaDiaria-{vendaDiariaId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int vendaDiariaId) => $"VendaDiariaDetails-{vendaDiariaId}";


        //---------------------------------------------------------------------------------------------------

    }
}