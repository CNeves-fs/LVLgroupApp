using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class ReportAnswer
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        public int QuestionTemplateId { get; set; }

        [StringLength(256)]
        public string Answer { get; set; } // Valor como string (parse conforme tipo)



        public virtual Report Report { get; set; }

        public QuestionTemplate QuestionTemplate { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
