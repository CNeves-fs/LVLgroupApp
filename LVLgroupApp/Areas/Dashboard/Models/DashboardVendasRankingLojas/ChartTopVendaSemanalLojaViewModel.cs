using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasRankingLojas
{
    public class ChartTopVendaSemanalLojaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string Index { get; set; }

        public string LojaNome { get; set; }

        public int Quantidade { get; set; }

        public double Valor { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}