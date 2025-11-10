using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class ReportTemplate : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int ReportTypeId { get; set; } // Ex: "Inspeção de Stock"

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }




        public virtual ReportType ReportType { get; set; }

        //public virtual ICollection<ReportTemplateQuestion> ReportTemplateQuestions { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
