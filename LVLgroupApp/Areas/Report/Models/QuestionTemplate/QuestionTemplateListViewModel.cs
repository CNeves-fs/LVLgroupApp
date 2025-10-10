using LVLgroupApp.Areas.Report.Models.QuestionOption;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.QuestionTemplate
{
    public class QuestionTemplateListViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string QuestionText { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public int QuestionTypeId { get; set; }

        public string QuestionTypeName { get; set; }

        public int UsedInReports { get; set; }

        public List<QuestionOptionViewModel> Options { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}