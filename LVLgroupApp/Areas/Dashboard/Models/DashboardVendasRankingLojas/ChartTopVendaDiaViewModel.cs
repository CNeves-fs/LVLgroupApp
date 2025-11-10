using Core.Entities.Charts;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasRankingLojas
{
    public class ChartTopVendaDiaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public DateTime DateDashboard { get; set; }

        public int Dia { get; set; }

        public int Mes { get; set; }

        public int Ano { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string DivId { get; set; }

        public List<ChartBarColumn> VendasDiariasColumnsList { get; set; }

        public List<ChartBarVendasRow> VendasDiariasRowsList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}