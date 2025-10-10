using System;

namespace LVLgroupApp.Areas.Vendas.Models.VendaMensal
{
    public class BoxMensalViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int MercadoId { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int Mes { get; set; }

        public string MesLiteral { get; set; }

        public int Ano { get; set; }            // relativo à start date

        public Double ValorTotalMensalDaVenda { get; set; }

        public Double ObjetivoMensalDaVenda { get; set; }

        public Double ValorAcumuladoMensal { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}