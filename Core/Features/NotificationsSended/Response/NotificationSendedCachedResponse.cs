namespace Core.Features.NotificationsSended.Response
{
    public class NotificationSendedCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int NotificationId { get; set; }

        public string ToUserId { get; set; }

        public bool IsRead { get; set; } = false;


        //---------------------------------------------------------------------------------------------------

    }
}