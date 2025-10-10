using Core.Abstractions;
using Core.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Core.Entities.Notifications
{
    public class Notification : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string FromUserId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }



        public virtual ApplicationUser FromUser { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
