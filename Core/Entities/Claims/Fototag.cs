using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Claims
{
    public class Fototag : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }
        
        [StringLength(100)]
        public string Tag { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
