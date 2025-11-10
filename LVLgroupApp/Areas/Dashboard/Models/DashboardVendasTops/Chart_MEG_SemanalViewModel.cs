using Core.Entities.Charts;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasTops
{
    public class Chart_MEG_SemanalViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int NumeroDaSemana { get; set; }

        public int Ano { get; set; }

        public List<ChartPointVendas> M_VendasDaSemanaList { get; set; } //Chart Mercado

        public List<ChartPointVendas> E_VendasDaSemanaList { get; set; } //Chart Empresa

        public List<ChartPointVendas> G1_VendasDaSemanaList { get; set; } //Chart Grupoloja

        public List<ChartPointVendas> G2_VendasDaSemanaList { get; set; } //Chart Grupoloja


        //---------------------------------------------------------------------------------------------------

    }
}