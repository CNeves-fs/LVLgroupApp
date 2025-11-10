using Core.Entities.Reports;
using System;
using System.Collections.Generic;

namespace Core.Features.QuestionOption.Response
{
    public class QuestionOptionCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string OptionText_pt { get; set; }

        public string OptionText_en { get; set; }

        public string OptionText_es { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}