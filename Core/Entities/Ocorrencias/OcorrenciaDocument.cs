using Core.Abstractions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Ocorrencias
{
    public class OcorrenciaDocument : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }                // Path no server /wwwroot/Ocorrencias/id/filename

        public int? OcorrenciaId { get; set; }              // null => Ocorrencia a ser criada ainda não tem Id

        [Required]
        public DateTime UploadDate { get; set; }

        [StringLength(255)]
        public string OcorrenciaFolder { get; set; }

        [StringLength(255)]
        public string Descrição { get; set; }



        public virtual Ocorrencia Ocorrencia { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
