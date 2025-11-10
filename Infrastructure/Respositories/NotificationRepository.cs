using Core.Entities.Business;
using Core.Entities.Notifications;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Notification> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public NotificationRepository(IDistributedCache distributedCache, IRepositoryAsync<Notification> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Notification> Notifications => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Notification>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Notification>> GetListFromUserIdAsync(string userId)
        {
            return await _repository.Entities.Where(n => n.FromUserId.Equals(userId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Notification> GetByIdAsync(int notificationId)
        {
            return await _repository.Entities.Where(n => n.Id == notificationId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Notification notification)
        {
            await _repository.AddAsync(notification);
            await _distributedCache.RemoveAsync(NotificationCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificationCacheKeys.GetKey(notification.Id));
            await _distributedCache.RemoveAsync(NotificationCacheKeys.ListFromUserIdKey(notification.FromUserId));
            return notification.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Notification notification)
        {
            await _repository.DeleteAsync(notification);
            await _distributedCache.RemoveAsync(NotificationCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificationCacheKeys.GetKey(notification.Id));
            await _distributedCache.RemoveAsync(NotificationCacheKeys.ListFromUserIdKey(notification.FromUserId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(Notification notification)
        {
            await _repository.UpdateAsync(notification);
            await _distributedCache.RemoveAsync(NotificationCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificationCacheKeys.GetKey(notification.Id));
            await _distributedCache.RemoveAsync(NotificationCacheKeys.ListFromUserIdKey(notification.FromUserId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}