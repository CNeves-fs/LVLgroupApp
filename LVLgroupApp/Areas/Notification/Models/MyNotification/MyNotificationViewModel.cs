using LVLgroupApp.Areas.Notification.Models.Notification;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Notification.Models.MyNotification
{
    public class MyNotificationViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int NotificationId { get; set; }

        public DateTime Date { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public FromUserDetailViewModel FromUser { get; set; }      // From ApplicationUser

        public NotificationSendedViewModel NotificationSended { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
