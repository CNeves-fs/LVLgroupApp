using System;

namespace Core.Features.VendasSemanais.Response
{
    public class VendaSemanalCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime DataInicialDaSemana { get; set; }

        public DateTime DataFinalDaSemana { get; set; }

        public int NumeroDaSemana { get; set; } = 0;

        public int Mes { get; set; }  // relativo à start date

        public int Quarter { get; set; }  // relativo à start date

        public int Ano { get; set; }  // ano a que pertence a semana

        public Double ValorTotalDaVenda { get; set; }

        public Double ValorTotalDaVendaDoAnoAnterior { get; set; }

        public Double ObjetivoDaVendaSemanal { get; set; }

        public Double VariaçaoAnual { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}