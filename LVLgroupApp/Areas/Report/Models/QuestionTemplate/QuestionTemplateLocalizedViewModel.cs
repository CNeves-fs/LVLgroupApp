using LVLgroupApp.Areas.Report.Models.QuestionOption;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.QuestionTemplate
{
    public class QuestionTemplateLocalizedViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string QuestionText { get; set; }

        public string Language { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}