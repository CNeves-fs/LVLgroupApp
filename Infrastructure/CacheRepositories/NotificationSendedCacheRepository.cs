using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Notifications;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Infrastructure.CacheRepositories
{
    public class NotificationSendedCacheRepository : INotificationSendedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly INotificationSendedRepository _notificationSendedRepository;


        //---------------------------------------------------------------------------------------------------


        public NotificationSendedCacheRepository(IDistributedCache distributedCache, INotificationSendedRepository notificationSendedRepository)
        {
            _distributedCache = distributedCache;
            _notificationSendedRepository = notificationSendedRepository;
        }



        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetCachedListAsync()
        {
            string cacheKey = NotificationSendedCacheKeys.ListKey;
            var notificationSendedList = await _distributedCache.GetAsync<List<NotificationSended>>(cacheKey);
            if (notificationSendedList == null)
            {
                notificationSendedList = await _notificationSendedRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, notificationSendedList);
            }
            return notificationSendedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetByToUserIdCachedListAsync(string userId)
        {
            string cacheKey = NotificationSendedCacheKeys.ListToUserIdKey(userId);
            var notificationSendedList = await _distributedCache.GetAsync<List<NotificationSended>>(cacheKey);
            if (notificationSendedList == null)
            {
                notificationSendedList = await _notificationSendedRepository.GetListToUserIdAsync(userId);
                await _distributedCache.SetAsync(cacheKey, notificationSendedList);
            }
            return notificationSendedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetByToUserIdNotReadCachedListAsync(string userId)
        {
            string cacheKey = NotificationSendedCacheKeys.ListToUserIdNotReadKey(userId);
            var notificationSendedList = await _distributedCache.GetAsync<List<NotificationSended>>(cacheKey);
            if (notificationSendedList == null)
            {
                notificationSendedList = await _notificationSendedRepository.GetNotReadListToUserIdAsync(userId);
                await _distributedCache.SetAsync(cacheKey, notificationSendedList);
            }
            return notificationSendedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetByNotificationIdCachedListAsync(int notificationId)
        {
            string cacheKey = NotificationSendedCacheKeys.ListFromNotificationIdKey(notificationId);
            var notificationSendedList = await _distributedCache.GetAsync<List<NotificationSended>>(cacheKey);
            if (notificationSendedList == null)
            {
                notificationSendedList = await _notificationSendedRepository.GetListNotificationIdAsync(notificationId);
                await _distributedCache.SetAsync(cacheKey, notificationSendedList);
            }
            return notificationSendedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<NotificationSended> GetByIdAsync(int notificationSendedId)
        {
            string cacheKey = NotificationSendedCacheKeys.GetKey(notificationSendedId);
            var notificationSended = await _distributedCache.GetAsync<NotificationSended>(cacheKey);
            if (notificationSended == null)
            {
                notificationSended = await _notificationSendedRepository.GetByIdAsync(notificationSendedId);
                // Throw.Exception.IfNull(notificationSended, "NotificationSended", "NotificationSended not Found");
                if (notificationSended == null) return null;
                await _distributedCache.SetAsync(cacheKey, notificationSended);
            }
            return notificationSended;
        }


        //---------------------------------------------------------------------------------------------------

    }
}