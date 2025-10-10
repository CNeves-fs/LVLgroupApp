using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Charts;
using Core.Entities.Clientes;
using Core.Interfaces.ChartCacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.ChartCacheRepositories
{
    public class ClienteChartCacheRepository : IClienteChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IClienteRepository _clienteRepository;


        //---------------------------------------------------------------------------------------------------


        public ClienteChartCacheRepository(IDistributedCache distributedCache, IClienteRepository clienteRepository)
        {
            _distributedCache = distributedCache;
            _clienteRepository = clienteRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountAllClientesCachedAsync()
        {
            string cacheKey = ClienteCacheKeys.ListKey;
            var clienteList = await _distributedCache.GetAsync<List<Cliente>>(cacheKey);
            if (clienteList == null)
            {
                clienteList = await _clienteRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, clienteList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = clienteList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------

    }
}