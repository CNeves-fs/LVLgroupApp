using Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LVLgroupApp.Views.Shared.Components.AnswerText.Models
{
    public class AnswerTextViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int stepIndex { get; set; }      // para fazer bind do answer em formulários com múltiplos steps

        public string QuestionText { get; set; }

        public string AnswerText { get; set; }

        //---------------------------------------------------------------------------------------------------

    }
}