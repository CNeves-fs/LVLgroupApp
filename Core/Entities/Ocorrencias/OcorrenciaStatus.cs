using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Ocorrencias
{
    public class OcorrenciaStatus
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }
        
        [StringLength(100)]
        public string Status { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
