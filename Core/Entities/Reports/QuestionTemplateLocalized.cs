using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class QuestionTemplateLocalized : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string QuestionText { get; set; }

        public string Language { get; set; }




        public virtual QuestionTemplate QuestionTemplate { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
