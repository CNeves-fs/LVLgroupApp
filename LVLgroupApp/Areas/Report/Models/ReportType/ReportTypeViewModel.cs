using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.ReportType
{
    public class ReportTypeViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string DefaultName { get; set; }

        public string EsName { get; set; }

        public string EnName { get; set; }

        public bool EditMode { get; set; }




        public ICollection<ReportTypeLocalizedViewModel> Translations { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
