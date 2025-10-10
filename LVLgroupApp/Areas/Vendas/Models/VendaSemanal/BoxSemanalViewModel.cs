using System;

namespace LVLgroupApp.Areas.Vendas.Models.VendaSemanal
{
    public class BoxSemanalViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int MercadoId { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int NumeroDaSemana { get; set; }

        public int Ano { get; set; }            // relativo à start date

        public Double ValorTotalDaVenda { get; set; }

        public Double ObjetivoDaVendaSemanal { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}