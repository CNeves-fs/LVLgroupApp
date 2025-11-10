using Core.Entities.Notifications;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class NotificationViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string FromUserId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
