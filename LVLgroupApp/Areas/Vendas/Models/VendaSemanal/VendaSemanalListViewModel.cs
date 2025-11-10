using System;

namespace LVLgroupApp.Areas.Vendas.Models.VendaSemanal
{
    public class VendaSemanalListViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime DataInicialDaSemana { get; set; }

        public DateTime DataFinalDaSemana { get; set; }

        public int NumeroDaSemana { get; set; } = 0;

        public int Mes { get; set; } = 0;  // relativo à start date

        public int Quarter { get; set; } = 0;  // relativo à start date

        public int Ano { get; set; } = 0;  // relativo à start date

        public Double ValorTotalDaVenda { get; set; } = 0;

        public Double ValorTotalDaVendaDoAnoAnterior { get; set; } = 0;

        public Double ObjetivoDaVendaSemanal { get; set; } = 0;

        public Double VariaçaoAnual { get; set; } = 0;

        public int EmpresaId { get; set; }

        public string EmpresaNome { get; set; }

        public string EmpresaLogo { get; set; }

        public int GrupolojaId { get; set; }

        public string GrupolojaNome { get; set; }

        public int LojaId { get; set; }

        public string LojaNome { get; set; }

        public int MercadoId { get; set; }

        public string MercadoNome { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}