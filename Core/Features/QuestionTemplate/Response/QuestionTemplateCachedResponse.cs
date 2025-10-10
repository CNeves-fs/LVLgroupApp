using System;

namespace Core.Features.QuestionTemplate.Response
{
    public class QuestionTemplateCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string QuestionText { get; set; }

        public int QuestionTypeId { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}