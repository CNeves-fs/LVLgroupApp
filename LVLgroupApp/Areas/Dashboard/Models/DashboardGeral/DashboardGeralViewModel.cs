using Core.Entities.Charts;
using DocumentFormat.OpenXml.Vml;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardGeral
{
    public class DashboardGeralViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int TotalClaims { get; set; }

        public int ClaimsPorFechar { get; set; }

        public int ClaimsPorDecidir { get; set; }

        public int FreeDiskSpace { get; set; }

        public List<ChartPoint> TotalClaimsList { get; set; } //Chart Total de Reclamações

        public ChartStatusViewModel StatusClaims { get; set; } //Chart Reclamações por Status


        //---------------------------------------------------------------------------------------------------

    }
}