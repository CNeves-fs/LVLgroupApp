using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class ReportType : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DefaultName { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
