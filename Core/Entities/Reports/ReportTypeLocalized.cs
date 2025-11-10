using Core.Abstractions;
using Core.Entities.Reports;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class ReportTypeLocalized : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        public int ReportTypeId { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }



        public virtual ReportType ReportType { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
