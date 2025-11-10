using Core.Entities.Charts;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasTops
{
    public class Chart_GL_SemanalViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int NumeroDaSemana { get; set; }

        public int Ano { get; set; }

        public string DivId { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public List<ChartBarColumn> VendasSemanaisColumnsList { get; set; } //Columns: Label + "GEOX" + "SKECHERS"

        public List<ChartBarVendasRow> VendasSemanaisRowsList { get; set; } //Row: Status + Value + Value


        //---------------------------------------------------------------------------------------------------

    }
}