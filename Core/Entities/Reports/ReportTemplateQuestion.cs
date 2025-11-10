using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class ReportTemplateQuestion
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        public int ReportTemplateId { get; set; }

        [Required]
        public int QuestionTemplateId { get; set; }

        [Required]
        //public int QuestionTypeId { get; set; }

        public int Order { get; set; }



        public virtual ReportTemplate ReportTemplate { get; set; }
        public virtual QuestionTemplate QuestionTemplate { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
