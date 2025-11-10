using Core.Abstractions;
using Core.Entities.Identity;
using System.Collections.Generic;

namespace Core.Entities.Notifications
{
    public class NotificationSended : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------
        // many-to-many relationship between Notification and ApplicationUser

        public int Id { get; set; }

        public int NotificationId { get; set; }

        public string ToUserId { get; set; }

        public bool IsRead { get; set; } = false;




        public virtual Notification Notification { get; set; }

        public virtual ApplicationUser ToUser { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
