using System.ComponentModel.DataAnnotations;

namespace Core.Features.QuestionTemplateLocalized.Response
{
    public class QuestionTemplateLocalizedCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string QuestionText { get; set; }

        public string Language { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}