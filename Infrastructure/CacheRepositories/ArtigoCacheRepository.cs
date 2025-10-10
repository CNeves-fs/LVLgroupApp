using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Artigos;
using Core.Entities.Clientes;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class ArtigoCacheRepository : IArtigoCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IArtigoRepository _artigoRepository;


        //---------------------------------------------------------------------------------------------------


        public ArtigoCacheRepository(IDistributedCache distributedCache, IArtigoRepository artigoRepository)
        {
            _distributedCache = distributedCache;
            _artigoRepository = artigoRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Artigo> GetByIdAsync(int artigoId)
        {
            string cacheKey = ArtigoCacheKeys.GetKey(artigoId);
            var artigo = await _distributedCache.GetAsync<Artigo>(cacheKey);
            if (artigo == null)
            {
                artigo = await _artigoRepository.GetByIdAsync(artigoId);
                //Throw.Exception.IfNull(artigo, "Artigo", "Artigo not Found");
                if (artigo == null) return null;
                await _distributedCache.SetAsync(cacheKey, artigo);
            }
            return artigo;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Artigo>> GetCachedListAsync()
        {
            string cacheKey = ArtigoCacheKeys.ListKey;
            var artigoList = await _distributedCache.GetAsync<List<Artigo>>(cacheKey);
            if (artigoList == null)
            {
                artigoList = await _artigoRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, artigoList);
            }
            return artigoList;
        }





        //---------------------------------------------------------------------------------------------------


        public async Task<List<Artigo>> GetByEmpresaCachedListAsync(int empresaId)
        {
            string cacheKey = ArtigoCacheKeys.ListFromEmpresaKey(empresaId);
            var artigoList = await _distributedCache.GetAsync<List<Artigo>>(cacheKey);
            if (artigoList == null)
            {
                artigoList = await _artigoRepository.GetListFromEmpresaAsync(empresaId);
                await _distributedCache.SetAsync(cacheKey, artigoList);
            }
            return artigoList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Artigo> GetByReferenciaAsync(string referencia)
        {
            string cacheKey = ArtigoCacheKeys.GetReferenciaKey(referencia);
            var artigo = await _distributedCache.GetAsync<Artigo>(cacheKey);
            if (artigo == null)
            {
                artigo = await _artigoRepository.GetByReferenciaAsync(referencia);
                if (artigo == null) return null;
                //Throw.Exception.IfNull(artigo, "Artigo", "Artigo not Found");
                await _distributedCache.SetAsync(cacheKey, artigo);
            }
            return artigo;
        }


        //---------------------------------------------------------------------------------------------------

    }
}