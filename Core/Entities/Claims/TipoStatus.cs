using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Claims
{
    public class TipoStatus
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }
        
        [StringLength(100)]
        public string Tipo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
