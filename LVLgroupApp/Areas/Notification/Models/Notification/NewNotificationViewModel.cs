using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class NewNotificationViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public FromUserDetailViewModel FromUser { get; set; }

        public DateTime Date { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public string ToUsers { get; set; }             // string of user ids separated by ";"

        public List<ToUserDetailViewModel> AllUsers { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
