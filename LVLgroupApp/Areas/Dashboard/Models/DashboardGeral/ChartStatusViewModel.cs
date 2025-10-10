using Core.Entities.Charts;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardGeral
{
    public class ChartStatusViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public List<ChartBarColumn> StatusColumnsList { get; set; } //Columns: Label + "GEOX" + "SKECHERS"

        public List<ChartBarRow> StatusRowsList { get; set; } //Row: Status + Value + Value


        //---------------------------------------------------------------------------------------------------

    }
}