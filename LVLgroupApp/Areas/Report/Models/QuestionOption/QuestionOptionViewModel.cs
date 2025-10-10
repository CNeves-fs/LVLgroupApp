using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Report.Models.QuestionOption
{
    public class QuestionOptionViewModel
    {

        //Opções para perguntas do tipo multichoice.
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
