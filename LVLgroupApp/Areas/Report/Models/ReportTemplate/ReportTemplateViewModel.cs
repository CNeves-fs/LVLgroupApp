using LVLgroupApp.Areas.Report.Models.QuestionTemplate;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.ReportTemplate
{
    public class ReportTemplateViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Name { get; set; }

        public int ReportTypeId { get; set; } // Ex: "Inspeção de Stock"

        public string ReportTypeName { get; set; } // Ex: "Inspeção de Stock"

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public int NumberOfQuestions { get; set; }

        public int UsedInReports { get; set; }

        public List<QuestionTemplateViewModel> QuestionTemplateList { get; set; }

        public SelectList ReportTypes { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}