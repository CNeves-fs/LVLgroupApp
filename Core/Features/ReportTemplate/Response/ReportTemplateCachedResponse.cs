using System;

namespace Core.Features.ReportTemplate.Response
{
    public class ReportTemplateCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Name { get; set; }

        public int ReportTypeId { get; set; } // Ex: "Inspeção de Stock"

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}