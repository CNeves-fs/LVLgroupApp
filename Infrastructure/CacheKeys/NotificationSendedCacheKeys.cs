using Core.Entities.Notifications;

namespace Infrastructure.CacheKeys
{
    public static class NotificationSendedCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "NotificationsSendedList";


        //---------------------------------------------------------------------------------------------------


        public static string ListToUserIdKey(string userId) => $"NotificationsSendedToUserList-{userId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListToUserIdNotReadKey(string userId) => $"NotificationsSendedToUserNotReadList-{userId}";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromNotificationIdKey(int notificationId) => $"NotificationsSendedFromNotificationList-{notificationId}";


        //---------------------------------------------------------------------------------------------------
        
        
        public static string GetKey(int notificationSendedId) => $"NotificationSended-{notificationSendedId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int notificationSendedId) => $"NotificationSendedDetails-{notificationSendedId}";


        //---------------------------------------------------------------------------------------------------

    }
}