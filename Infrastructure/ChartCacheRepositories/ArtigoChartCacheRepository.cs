using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Artigos;
using Core.Entities.Charts;
using Core.Interfaces.ChartCacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.ChartCacheRepositories
{
    public class ArtigoChartCacheRepository : IArtigoChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IArtigoRepository _artigoRepository;


        //---------------------------------------------------------------------------------------------------


        public ArtigoChartCacheRepository(IDistributedCache distributedCache, IArtigoRepository artigoRepository)
        {
            _distributedCache = distributedCache;
            _artigoRepository = artigoRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountAllArtigosCachedAsync()
        {
            string cacheKey = ArtigoCacheKeys.ListKey;
            var artigoList = await _distributedCache.GetAsync<List<Artigo>>(cacheKey);
            if (artigoList == null)
            {
                artigoList = await _artigoRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, artigoList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = artigoList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------

    }
}