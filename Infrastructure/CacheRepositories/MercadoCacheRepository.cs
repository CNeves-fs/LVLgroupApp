using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Business;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class MercadoCacheRepository : IMercadoCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IMercadoRepository _mercadoRepository;


        //---------------------------------------------------------------------------------------------------


        public MercadoCacheRepository(IDistributedCache distributedCache, IMercadoRepository mercadoRepository)
        {
            _distributedCache = distributedCache;
            _mercadoRepository = mercadoRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Mercado> GetByIdAsync(int mercadoId)
        {
            string cacheKey = MercadoCacheKeys.GetKey(mercadoId);
            var mercado = await _distributedCache.GetAsync<Mercado>(cacheKey);
            if (mercado == null)
            {
                mercado = await _mercadoRepository.GetByIdAsync(mercadoId);
                // Throw.Exception.IfNull(mercado, "Mercado", "Mercado not Found");
                if (mercado == null) return null;
                await _distributedCache.SetAsync(cacheKey, mercado);
            }
            return mercado;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Mercado> GetByNomeAsync(string nome)
        {
            string cacheKey = MercadoCacheKeys.GetNomeKey(nome);
            var mercado = await _distributedCache.GetAsync<Mercado>(cacheKey);
            if (mercado == null)
            {
                mercado = await _mercadoRepository.GetByNomeAsync(nome);
                //Throw.Exception.IfNull(mercado, "Mercado", "Mercado not Found");
                if (mercado == null) return null;
                await _distributedCache.SetAsync(cacheKey, mercado);
            }
            return mercado;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Mercado>> GetCachedListAsync()
        {
            string cacheKey = MercadoCacheKeys.ListKey;
            var mercadoList = await _distributedCache.GetAsync<List<Mercado>>(cacheKey);
            if (mercadoList == null)
            {
                mercadoList = await _mercadoRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, mercadoList);
            }
            return mercadoList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}