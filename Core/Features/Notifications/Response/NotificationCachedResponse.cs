using System;

namespace Core.Features.Notifications.Response
{
    public class NotificationCachedResponse
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