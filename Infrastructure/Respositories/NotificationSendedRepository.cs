using Core.Entities.Notifications;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationSendedRepository : INotificationSendedRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<NotificationSended> _repository;

        private readonly IDistributedCache _distributedCache;

        //private readonly IHubContext<NotificationHub> _hubContext;


        //---------------------------------------------------------------------------------------------------


        public NotificationSendedRepository(IDistributedCache distributedCache
                                           ,IRepositoryAsync<NotificationSended> repository
                                           //,IHubContext<NotificationHub> hubContext
                                           )
        {
            _distributedCache = distributedCache;
            _repository = repository;
            //_hubContext = hubContext;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<NotificationSended> NotificationsSended => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetListToUserIdAsync(string userId)
        {
            return await _repository.Entities.Where(n => n.ToUserId.Equals(userId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetNotReadListAsync()
        {
            return await _repository.Entities.Where(n => !n.IsRead).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetNotReadListToUserIdAsync(string userId)
        {
            return await _repository.Entities.Where(n => !n.IsRead && (n.ToUserId.Equals(userId))).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificationSended>> GetListNotificationIdAsync(int notificationId)
        {
            return await _repository.Entities.Where(n => n.NotificationId == notificationId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<NotificationSended> GetByIdAsync(int notificationSendedId)
        {
            return await _repository.Entities.Where(n => n.Id == notificationSendedId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(NotificationSended notificationSended)
        {
            await _repository.AddAsync(notificationSended);
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.GetKey(notificationSended.Id));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListToUserIdKey(notificationSended.ToUserId));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListToUserIdNotReadKey(notificationSended.ToUserId));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListFromNotificationIdKey(notificationSended.NotificationId));
            
            

            return notificationSended.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(NotificationSended notificationSended)
        {
            await _repository.DeleteAsync(notificationSended);
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.GetKey(notificationSended.Id));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListToUserIdKey(notificationSended.ToUserId));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListToUserIdNotReadKey(notificationSended.ToUserId));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListFromNotificationIdKey(notificationSended.NotificationId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(NotificationSended notificationSended)
        {
            await _repository.UpdateAsync(notificationSended);
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.GetKey(notificationSended.Id));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListToUserIdKey(notificationSended.ToUserId));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListToUserIdNotReadKey(notificationSended.ToUserId));
            await _distributedCache.RemoveAsync(NotificationSendedCacheKeys.ListFromNotificationIdKey(notificationSended.NotificationId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}