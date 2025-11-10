using Core.Entities.Charts;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral
{
    public class ChartSemanalViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string Title { get; set; } //Title: "SKECHERS / CANÁRIS"

        public string Subtitle { get; set; } //Subtitle: "Vendas por mês"

        public List<ChartBarColumn> VendasSemanaisColumnsList { get; set; } //Columns: Label + "GEOX" + "SKECHERS"

        public List<ChartBarVendasRow> VendasSemanaisRowsList { get; set; } //Row: Status + Value + Value


        //---------------------------------------------------------------------------------------------------

    }
}