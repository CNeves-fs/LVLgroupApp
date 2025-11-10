using System;

namespace Core.Features.ReportTemplateQuestion.Response
{
    public class ReportTemplateQuestionCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        public int QuestionTemplateId { get; set; }

        public int QuestionTypeId { get; set; }

        public int Order { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}