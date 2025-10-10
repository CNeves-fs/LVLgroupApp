using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Infrastructure.CacheRepositories
{
    public class StatusCacheRepository : IStatusCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IStatusRepository _statusRepository;


        //---------------------------------------------------------------------------------------------------


        public StatusCacheRepository(IDistributedCache distributedCache, IStatusRepository statusRepository)
        {
            _distributedCache = distributedCache;
            _statusRepository = statusRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Status> GetByIdAsync(int statusId)
        {
            string cacheKey = StatusCacheKeys.GetKey(statusId);
            var status = await _distributedCache.GetAsync<Status>(cacheKey);
            if (status == null)
            {
                status = await _statusRepository.GetByIdAsync(statusId);
                // Throw.Exception.IfNull(status, "Status", "Status not Found");
                if (status == null) return null;
                await _distributedCache.SetAsync(cacheKey, status);
            }
            return status;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Status>> GetCachedListAsync()
        {
            string cacheKey = StatusCacheKeys.ListKey;
            var statusList = await _distributedCache.GetAsync<List<Status>>(cacheKey);
            if (statusList == null)
            {
                statusList = await _statusRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, statusList);
            }
            return statusList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Status>> GetCachedTipoListAsync(int tipo)
        {
            string cacheKey = StatusCacheKeys.ListFromTypeKey(tipo);
            var statusList = await _distributedCache.GetAsync<List<Status>>(cacheKey);
            if (statusList == null)
            {
                statusList = await _statusRepository.GetTipoListAsync(tipo);
                await _distributedCache.SetAsync(cacheKey, statusList);
            }
            return statusList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}