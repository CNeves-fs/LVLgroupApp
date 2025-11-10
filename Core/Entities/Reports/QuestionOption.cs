using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class QuestionOption : AuditableEntity
    {

        //Opções para perguntas do tipo multichoice.
        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        public int QuestionTemplateId { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionText_pt { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionText_es { get; set; }

        [Required]
        [StringLength(100)]
        public string OptionText_en { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }



        public virtual QuestionTemplate QuestionTemplate { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
