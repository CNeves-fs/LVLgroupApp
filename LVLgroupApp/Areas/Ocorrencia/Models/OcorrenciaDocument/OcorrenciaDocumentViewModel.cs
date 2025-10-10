using System;

namespace LVLgroupApp.Areas.Ocorrencia.Models.OcorrenciaDocument
{
    public class OcorrenciaDocumentViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }                // Path no server /wwwroot/Ocorrencias/id/filename

        public int? OcorrenciaId { get; set; }              // null => Ocorrencia a ser criada ainda não tem Id

        public DateTime UploadDate { get; set; }

        public string OcorrenciaFolder { get; set; }

        public string Descrição { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
