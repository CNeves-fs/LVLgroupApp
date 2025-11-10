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
    public class PrazolimiteCacheRepository : IPrazolimiteCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IPrazolimiteRepository _prazolimiteRepository;


        //---------------------------------------------------------------------------------------------------


        public PrazolimiteCacheRepository(IDistributedCache distributedCache, IPrazolimiteRepository prazolimiteRepository)
        {
            _distributedCache = distributedCache;
            _prazolimiteRepository = prazolimiteRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Prazolimite> GetByIdAsync(int prazolimiteId)
        {
            string cacheKey = PrazolimiteCacheKeys.GetKey(prazolimiteId);
            var prazolimite = await _distributedCache.GetAsync<Prazolimite>(cacheKey);
            if (prazolimite == null)
            {
                prazolimite = await _prazolimiteRepository.GetByIdAsync(prazolimiteId);
                // Throw.Exception.IfNull(prazolimite, "Prazolimite", "Prazo limite not Found");
                if (prazolimite == null) return null;
                await _distributedCache.SetAsync(cacheKey, prazolimite);
            }
            return prazolimite;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Prazolimite>> GetCachedListAsync()
        {
            string cacheKey = PrazolimiteCacheKeys.ListKey;
            var prazolimiteList = await _distributedCache.GetAsync<List<Prazolimite>>(cacheKey);
            if (prazolimiteList == null)
            {
                prazolimiteList = await _prazolimiteRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, prazolimiteList);
            }
            return prazolimiteList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}