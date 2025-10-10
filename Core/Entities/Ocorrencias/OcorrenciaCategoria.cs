using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Ocorrencias
{
    public class OcorrenciaCategoria
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }
        
        [StringLength(100)]
        public string Categoria { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
