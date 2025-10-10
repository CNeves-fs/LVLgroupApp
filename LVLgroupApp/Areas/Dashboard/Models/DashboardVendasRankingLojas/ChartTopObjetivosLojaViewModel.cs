using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasRankingLojas
{
    public class ChartTopObjetivosLojaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string Index { get; set; }

        public string LojaNome { get; set; }

        public int Semanas { get; set; }

        public double Variacao { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}