using Core.Entities.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface INotificationSendedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<NotificationSended>> GetCachedListAsync();

        Task<List<NotificationSended>> GetByToUserIdCachedListAsync(string userId);

        Task<List<NotificationSended>> GetByToUserIdNotReadCachedListAsync(string userId);

        Task<List<NotificationSended>> GetByNotificationIdCachedListAsync(int notificationId);

        Task<NotificationSended> GetByIdAsync(int notificationSendedId);


        //---------------------------------------------------------------------------------------------------

    }
}