using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral
{
    public class DashboardVendasDiaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public DateTime DataDashboard { get; set; }

        public DateTime AA_DataDashboard { get; set; }

        public List<VendasDayViewModel> VendasPorDiaList { get; set; }

        public List<VendasDayViewModel> AA_VendasPorDiaList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}