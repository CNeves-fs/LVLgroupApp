using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Select2
{
    public class Select2User
    {

        //---------------------------------------------------------------------------------------------------


        public string id { get; set; }

        public string text { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RoleName { get; set; }

        public string Local { get; set; }

        public byte[] ProfilePicture { get; set; }

        public bool selected { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}