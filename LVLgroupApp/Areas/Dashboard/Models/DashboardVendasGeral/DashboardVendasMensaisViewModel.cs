using Core.Entities.Charts;
using Core.Entities.Identity;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral
{
    public class DashboardVendasMensaisViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public DateTime DataDashboard { get; set; }

        public DateTime AA_DataDashboard { get; set; }

        public List<VendasMonthViewModel> VendasPorMesList { get; set; }

        public List<VendasMonthViewModel> AA_VendasPorMesList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}