using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Business;
using Core.Entities.Charts;
using Core.Interfaces.ChartCacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.ChartCacheRepositories
{
    public class LojaChartCacheRepository : ILojaChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly ILojaRepository _lojaRepository;


        //---------------------------------------------------------------------------------------------------


        public LojaChartCacheRepository(IDistributedCache distributedCache, ILojaRepository lojaRepository)
        {
            _distributedCache = distributedCache;
            _lojaRepository = lojaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountAllLojasCachedAsync()
        {
            string cacheKey = LojaCacheKeys.ListKey;
            var lojaList = await _distributedCache.GetAsync<List<Loja>>(cacheKey);
            if (lojaList == null)
            {
                lojaList = await _lojaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, lojaList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = lojaList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------

    }
}