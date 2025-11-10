using Core.Entities.Identity;
using Core.Entities.Reports;
using Core.Enums;
using LVLgroupApp.Areas.Report.Models.QuestionOption;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Report.Models.Report
{
    public class ReportAnswerViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Language { get; set; }

        public int ReportId { get; set; }

        public int QuestionTemplateId { get; set; }

        public int QuestionTypeId { get; set; }

        public string QuestionTypeName { get; set; }

        public string QuestionText { get; set; }

        public List<QuestionOptionViewModel> Options { get; set; }

        public string AnswerText { get; set; } // Valor como string (parse conforme tipo)

        public bool EditMode { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}