using LVLgroupApp.Areas.Notification.Models.Notification;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Notification.Models.MyNotification
{
    public class NotificationBoxViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public List<ViewNotificationViewModel> InBox { get; set; }

        public List<ViewNotificationViewModel> OutBox { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
