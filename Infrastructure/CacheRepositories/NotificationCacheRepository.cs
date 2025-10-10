using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Notifications;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class NotificationCacheRepository : INotificationCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly INotificationRepository _notificationRepository;


        //---------------------------------------------------------------------------------------------------


        public NotificationCacheRepository(IDistributedCache distributedCache, INotificationRepository notificationRepository)
        {
            _distributedCache = distributedCache;
            _notificationRepository = notificationRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Notification> GetByIdAsync(int notificationId)
        {
            string cacheKey = NotificationCacheKeys.GetKey(notificationId);
            var notification = await _distributedCache.GetAsync<Notification>(cacheKey);
            if (notification == null)
            {
                notification = await _notificationRepository.GetByIdAsync(notificationId);
                // Throw.Exception.IfNull(notification, "Notification", "Notification not Found");
                if (notification == null) return null;
                await _distributedCache.SetAsync(cacheKey, notification);
            }
            return notification;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Notification>> GetCachedListAsync()
        {
            string cacheKey = NotificationCacheKeys.ListKey;
            var notificationList = await _distributedCache.GetAsync<List<Notification>>(cacheKey);
            if (notificationList == null)
            {
                notificationList = await _notificationRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, notificationList);
            }
            return notificationList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Notification>> GetByFromUserIdCachedListAsync(string userId)
        {
            string cacheKey = NotificationCacheKeys.ListFromUserIdKey(userId);
            var notificationList = await _distributedCache.GetAsync<List<Notification>>(cacheKey);
            if (notificationList == null)
            {
                notificationList = await _notificationRepository.GetListFromUserIdAsync(userId);
                await _distributedCache.SetAsync(cacheKey, notificationList);
            }
            return notificationList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}