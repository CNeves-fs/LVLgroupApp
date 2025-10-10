using Core.Entities.Business;
using Core.Entities.Vendas;

namespace Infrastructure.CacheKeys
{
    public static class VendaSemanalCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "VendasSemanaisList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromSemanaKey(int ano, int numeroDaSemana) => $"VendasSemanaisFromSemanaList-{ano}-{numeroDaSemana}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromAnoKey(int ano) => $"VendasSemanaisFromAnoList-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaKey(int lojaId) => $"VendasSemanaisFromLojaList-{lojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaSemanaKey(int lojaId, int ano, int numeroDaSemana) => $"VendasSemanaisFromLojaSemanaList-{lojaId}-{ano}-{numeroDaSemana}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaMesKey(int lojaId, int ano, int mes) => $"VendasSemanaisFromLojaMesList-{lojaId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaQuarterKey(int lojaId, int ano, int quarter) => $"VendasSemanaisFromLojaQuarterList-{lojaId}-{ano}-{quarter}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromLojaAnoKey(int lojaId, int ano) => $"VendasSemanaisFromLojaAnoList-{lojaId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoKey(int mercadoId) => $"VendasSemanaisFromMercadoList-{mercadoId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoSemanaKey(int mercadoId, int ano, int numeroDaSemana) => $"VendasSemanaisFromMercadoSemanaList-{mercadoId}-{ano}-{numeroDaSemana}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoMesKey(int mercadoId, int ano, int mes) => $"VendasSemanaisFromMercadoMesList-{mercadoId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoQuarterKey(int mercadoId, int ano, int quarter) => $"VendasSemanaisFromMercadoQuarterList-{mercadoId}-{ano}-{quarter}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromMercadoAnoKey(int mercadoId, int ano) => $"VendasSemanaisFromMercadoAnoList-{mercadoId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaKey(int empresaId) => $"VendasSemanaisFromEmpresaList-{empresaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaSemanaKey(int empresaId, int ano, int numeroDaSemana) => $"VendasSemanaisFromEmpresaSemanaList-{empresaId}-{ano}-{numeroDaSemana}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaMesKey(int empresaId, int ano, int mes) => $"VendasSemanaisFromEmpresaMesList-{empresaId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaQuarterKey(int empresaId, int ano, int quarter) => $"VendasSemanaisFromEmpresaQuarterList-{empresaId}-{ano}-{quarter}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromEmpresaAnoKey(int empresaId, int ano) => $"VendasSemanaisFromEmpresaAnoList-{empresaId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaKey(int grupolojaId) => $"VendasSemanaisFromGrupolojaList-{grupolojaId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaSemanaKey(int grupolojaId, int ano, int numeroDaSemana) => $"VendasSemanaisFromGrupolojaSemanaList-{grupolojaId}-{ano}-{numeroDaSemana}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaMesKey(int grupolojaId, int ano, int mes) => $"VendasSemanaisFromGrupolojaMesList-{grupolojaId}-{ano}-{mes}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaQuarterKey(int grupolojaId, int ano, int quarter) => $"VendasSemanaisFromGrupolojaQuarterList-{grupolojaId}-{ano}-{quarter}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromGrupolojaAnoKey(int grupolojaId, int ano) => $"VendasSemanaisFromGrupolojaAnoList-{grupolojaId}-{ano}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int vendaSemanalId) => $"VendaSemanal-{vendaSemanalId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int vendaSemanalId) => $"VendaSemanalDetails-{vendaSemanalId}";


        //---------------------------------------------------------------------------------------------------

    }
}