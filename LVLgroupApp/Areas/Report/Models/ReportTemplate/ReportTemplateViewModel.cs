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

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public int ReportTypeId { get; set; }

        public string ReportTypeName { get; set; } // Ex: "Inspeção de Stock"

        public SelectList ReportTypes { get; set; }

        // Lista das perguntas que pertencem ao relatório (binding para POST)
        public List<ReportTemplateQuestionViewModel> QuestionTemplateInReportList { get; set; }

        public DateTime CreatedAt { get; set; }

        public int NumberOfQuestions { get; set; }

        public int UsedInReports { get; set; }

        public List<QuestionTemplateViewModel> QuestionTemplateList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}