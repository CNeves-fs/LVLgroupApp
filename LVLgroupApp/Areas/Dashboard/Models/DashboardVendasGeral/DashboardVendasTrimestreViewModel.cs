using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral
{
    public class DashboardVendasTrimestreViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public DateTime DataDashboard { get; set; }

        public DateTime AA_DataDashboard { get; set; }

        public List<VendasQuarterViewModel> VendasPorQuarterList { get; set; }

        public List<VendasQuarterViewModel> AA_VendasPorQuarterList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}