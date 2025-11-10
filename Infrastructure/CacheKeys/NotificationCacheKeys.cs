namespace Infrastructure.CacheKeys
{
    public static class NotificationCacheKeys
    {

        //---------------------------------------------------------------------------------------------------


        public static string ListKey => "NotificationsList";


        //---------------------------------------------------------------------------------------------------


        public static string ListFromUserIdKey(string userId) => $"NotificationsFromUserList-{userId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetKey(int notificationId) => $"Notification-{notificationId}";


        //---------------------------------------------------------------------------------------------------


        public static string GetDetailsKey(int notificationId) => $"NotificationDetails-{notificationId}";


        //---------------------------------------------------------------------------------------------------

    }
}