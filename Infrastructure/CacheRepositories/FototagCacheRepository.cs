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
    public class FototagCacheRepository : IFototagCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IFototagRepository _fototagRepository;


        //---------------------------------------------------------------------------------------------------


        public FototagCacheRepository(IDistributedCache distributedCache, IFototagRepository fototagRepository)
        {
            _distributedCache = distributedCache;
            _fototagRepository = fototagRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Fototag> GetByIdAsync(int fototagId)
        {
            string cacheKey = FototagCacheKeys.GetKey(fototagId);
            var fototag = await _distributedCache.GetAsync<Fototag>(cacheKey);
            if (fototag == null)
            {
                fototag = await _fototagRepository.GetByIdAsync(fototagId);
                // Throw.Exception.IfNull(fototag, "Fototag", "Photo tag not Found");
                if (fototag == null) return null;
                await _distributedCache.SetAsync(cacheKey, fototag);
            }
            return fototag;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Fototag>> GetCachedListAsync()
        {
            string cacheKey = FototagCacheKeys.ListKey;
            var fototagList = await _distributedCache.GetAsync<List<Fototag>>(cacheKey);
            if (fototagList == null)
            {
                fototagList = await _fototagRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, fototagList);
            }
            return fototagList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}