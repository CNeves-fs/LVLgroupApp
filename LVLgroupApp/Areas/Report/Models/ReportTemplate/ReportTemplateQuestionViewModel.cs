using LVLgroupApp.Areas.Report.Models.QuestionTemplate;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.ReportTemplate
{
    public class ReportTemplateQuestionViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        public int QuestionTemplateId { get; set; }

        public int QuestionTypeId { get; set; }

        public int Order { get; set; }

        public QuestionTemplateViewModel QuestionTemplate { get; set; } // inclui traduções


        //---------------------------------------------------------------------------------------------------

    }
}