using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LVLgroupApp.Areas.Admin.Models
{
    public class SelectUserViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte[] ProfilePicture { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}