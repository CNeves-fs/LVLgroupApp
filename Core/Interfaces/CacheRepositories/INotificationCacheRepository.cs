using Core.Entities.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface INotificationCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Notification>> GetCachedListAsync();

        Task<List<Notification>> GetByFromUserIdCachedListAsync(string userId);

        Task<Notification> GetByIdAsync(int notificationId);


        //---------------------------------------------------------------------------------------------------

    }
}