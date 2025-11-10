namespace LVLgroupApp.Areas.Notification.Models.Notification
{
    public class NotificationSendedViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }                 // NotificationSendedId

        public int NotificationId { get; set; }     // NotificationId

        public string ToUserId { get; set; }

        public bool IsRead { get; set; } = false;


        //---------------------------------------------------------------------------------------------------

    }
}
