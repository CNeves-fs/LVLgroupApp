using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Clientes
{
    public class TipoContacto
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }
        
        [StringLength(100)]
        public string Tipo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
