using System.Collections.Generic;

namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class SystemNotificationDetailViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationViewModel Notification { get; set; }     // Notification

        public FromUserDetailViewModel FromUser { get; set; }       // Notification.FromUserId

        public List<ToUserDetailViewModel> ToUsers { get; set; }    // NotificationSended.ToUserId


        //---------------------------------------------------------------------------------------------------

    }
}
