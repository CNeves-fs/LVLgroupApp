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
    public class LojaCacheRepository : ILojaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly ILojaRepository _lojaRepository;


        //---------------------------------------------------------------------------------------------------


        public LojaCacheRepository(IDistributedCache distributedCache, ILojaRepository lojaRepository)
        {
            _distributedCache = distributedCache;
            _lojaRepository = lojaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Loja> GetByIdAsync(int lojaId)
        {
            string cacheKey = LojaCacheKeys.GetKey(lojaId);
            var loja = await _distributedCache.GetAsync<Loja>(cacheKey);
            if (loja == null)
            {
                loja = await _lojaRepository.GetByIdAsync(lojaId);
                // Throw.Exception.IfNull(loja, "Loja", "Loja not Found");
                if (loja == null) return null;
                await _distributedCache.SetAsync(cacheKey, loja);
            }
            return loja;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Loja> GetByNomeAsync(string nome)
        {
            var loja = await _lojaRepository.GetByNomeAsync(nome);
            // Throw.Exception.IfNull(loja, "Loja", "Loja not Found");

            return loja;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetCachedAllLojasListAsync()
        {
            string cacheKey = LojaCacheKeys.AllLojasListKey;
            var lojaList = await _distributedCache.GetAsync<List<Loja>>(cacheKey);
            if (lojaList == null)
            {
                lojaList = await _lojaRepository.GetAllLojasListAsync();
                await _distributedCache.SetAsync(cacheKey, lojaList);
            }
            return lojaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetCachedListAsync()
        {
            string cacheKey = LojaCacheKeys.ListKey;
            var lojaList = await _distributedCache.GetAsync<List<Loja>>(cacheKey);
            if (lojaList == null)
            {
                lojaList = await _lojaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, lojaList);
            }
            return lojaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetByGrupolojaIdCachedListAsync(int grupolojaId)
        {
            string cacheKey = LojaCacheKeys.ListFromGrupolojaKey(grupolojaId);
            var lojaList = await _distributedCache.GetAsync<List<Loja>>(cacheKey);
            if (lojaList == null)
            {
                lojaList = await _lojaRepository.GetListFromGrupolojaAsync(grupolojaId);
                await _distributedCache.SetAsync(cacheKey, lojaList);
            }
            return lojaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetByEmpresaIdCachedListAsync(int empresaId)
        {
            string cacheKey = LojaCacheKeys.ListFromEmpresaKey(empresaId);
            var lojaList = await _distributedCache.GetAsync<List<Loja>>(cacheKey);
            if (lojaList == null)
            {
                lojaList = await _lojaRepository.GetListFromEmpresaAsync(empresaId);
                await _distributedCache.SetAsync(cacheKey, lojaList);
            }
            return lojaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetByMercadoIdCachedListAsync(int? mercadoId)
        {
            if (mercadoId == null) return null;

            string cacheKey = LojaCacheKeys.ListFromMercadoKey(mercadoId);
            var lojaList = await _distributedCache.GetAsync<List<Loja>>(cacheKey);
            if (lojaList == null)
            {
                lojaList = await _lojaRepository.GetListFromMercadoAsync(mercadoId);
                await _distributedCache.SetAsync(cacheKey, lojaList);
            }
            return lojaList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}