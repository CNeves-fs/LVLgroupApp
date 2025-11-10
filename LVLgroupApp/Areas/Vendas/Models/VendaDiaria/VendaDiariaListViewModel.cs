using Core.Enums;
using LVLgroupApp.Areas.Claim.Models.Prazolimite;
using LVLgroupApp.Areas.Claim.Models.Status;
using System;

namespace LVLgroupApp.Areas.Vendas.Models.VendaDiaria
{
    public class VendaDiariaListViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime DataDaVenda { get; set; } = DateTime.Now;

        public int NumeroDaSemana { get; set; } = 0;

        public int Mes { get; set; } = 0;  // relativo à start date

        public int Quarter { get; set; } = 0;  // relativo à start date

        public int Ano { get; set; } = 0;  // relativo à start date

        public int VendaSemanalId { get; set; }

        public int DiaDaSemana { get; set; } = (int)DateTime.Now.DayOfWeek;

        public Double ValorDaVenda { get; set; } = 0;

        public Double PercentConv { get; set; } = 0;

        public int TotalArtigos { get; set; } = 0;

        public Weather Weather { get; set; }

        public string Observacoes { get; set; }




        public int LojaId { get; set; }

        public string LojaNome { get; set; }

        public int GrupolojaId { get; set; }

        public string GrupolojaNome { get; set; }

        public int EmpresaId { get; set; }

        public string EmpresaNome { get; set; }

        public string EmpresaLogo { get; set; }

        public int MercadoId { get; set; }

        public string MercadoNome { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}