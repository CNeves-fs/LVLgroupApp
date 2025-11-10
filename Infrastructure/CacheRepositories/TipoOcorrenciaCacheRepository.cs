using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Ocorrencias;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class TipoOcorrenciaCacheRepository : ITipoOcorrenciaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly ITipoOcorrenciaRepository _tipoOcorrenciaRepository;


        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaCacheRepository(IDistributedCache distributedCache, ITipoOcorrenciaRepository tipoOcorrenciaRepository)
        {
            _distributedCache = distributedCache;
            _tipoOcorrenciaRepository = tipoOcorrenciaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrencia>> GetCachedListAsync()
        {
            string cacheKey = TipoOcorrenciaCacheKeys.ListKey;
            var tipoOcorrenciaList = await _distributedCache.GetAsync<List<TipoOcorrencia>>(cacheKey);
            if (tipoOcorrenciaList == null)
            {
                tipoOcorrenciaList = await _tipoOcorrenciaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaList);
            }
            return tipoOcorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrencia>> GetByCategoriaIdCachedListAsync(int categoriaId)
        {
            string cacheKey = TipoOcorrenciaCacheKeys.GetCategoriaIdKey(categoriaId);
            var tipoOcorrenciaList = await _distributedCache.GetAsync<List<TipoOcorrencia>>(cacheKey);
            if (tipoOcorrenciaList == null)
            {
                tipoOcorrenciaList = await _tipoOcorrenciaRepository.GetByCategoriaIdListAsync(categoriaId);
                await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaList);
            }
            return tipoOcorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrencia> GetByIdAsync(int tipoOcorrenciaId)
        {
            string cacheKey = TipoOcorrenciaCacheKeys.GetKey(tipoOcorrenciaId);
            var tipoOcorrencia = await _distributedCache.GetAsync<TipoOcorrencia>(cacheKey);
            if (tipoOcorrencia == null)
            {
                tipoOcorrencia = await _tipoOcorrenciaRepository.GetByIdAsync(tipoOcorrenciaId);
                if (tipoOcorrencia == null) return null;
                await _distributedCache.SetAsync(cacheKey, tipoOcorrencia);
            }
            return tipoOcorrencia;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrencia> GetByNameAsync(string defaultname)
        {
            string cacheKey = TipoOcorrenciaCacheKeys.GetDefaultNameKey(defaultname);
            var tipoOcorrencia = await _distributedCache.GetAsync<TipoOcorrencia>(cacheKey);
            if (tipoOcorrencia == null)
            {
                tipoOcorrencia = await _tipoOcorrenciaRepository.GetByDefaultNameAsync(defaultname);
                if (tipoOcorrencia == null) return null;
                await _distributedCache.SetAsync(cacheKey, tipoOcorrencia);
            }
            return tipoOcorrencia;
        }


        //---------------------------------------------------------------------------------------------------

    }
}