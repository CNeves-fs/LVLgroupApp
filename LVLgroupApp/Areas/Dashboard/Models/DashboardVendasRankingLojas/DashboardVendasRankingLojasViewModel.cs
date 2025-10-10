using Core.Entities.Identity;
using System;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasRankingLojas
{
    public class DashboardVendasRankingLojasViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public CurrentRole CurrentRole { get; set; }

        public DateTime DataDashboard { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public int Dia { get; set; }

        public int NumeroDaSemana { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}