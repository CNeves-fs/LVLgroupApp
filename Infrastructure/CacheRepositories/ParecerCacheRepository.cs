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
    public class ParecerCacheRepository : IParecerCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IParecerRepository _parecerRepository;


        //---------------------------------------------------------------------------------------------------


        public ParecerCacheRepository(IDistributedCache distributedCache, IParecerRepository parecerRepository)
        {
            _distributedCache = distributedCache;
            _parecerRepository = parecerRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Parecer> GetByIdAsync(int parecerId)
        {
            string cacheKey = ParecerCacheKeys.GetKey(parecerId);
            var parecer = await _distributedCache.GetAsync<Parecer>(cacheKey);
            if (parecer == null)
            {
                parecer = await _parecerRepository.GetByIdAsync(parecerId);
                // Throw.Exception.IfNull(parecer, "Parecer", "Parecer not Found");
                if (parecer == null) return null;
                await _distributedCache.SetAsync(cacheKey, parecer);
            }
            return parecer;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Parecer>> GetCachedListAsync()
        {
            string cacheKey = ParecerCacheKeys.ListKey;
            var parecerList = await _distributedCache.GetAsync<List<Parecer>>(cacheKey);
            if (parecerList == null)
            {
                parecerList = await _parecerRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, parecerList);
            }
            return parecerList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}