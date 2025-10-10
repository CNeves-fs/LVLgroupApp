using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class ForgotPasswordRequest
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        [EmailAddress]
        public string Email { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}