using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class QuestionTemplate : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string QuestionText { get; set; }

        [Required]
        public int QuestionTypeId { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }



        //public virtual QuestionType QuestionType { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
