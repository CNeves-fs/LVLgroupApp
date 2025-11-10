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
    public class OcorrenciaCacheRepository : IOcorrenciaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IOcorrenciaRepository _ocorrenciaRepository;


        //---------------------------------------------------------------------------------------------------


        public OcorrenciaCacheRepository(IDistributedCache distributedCache, IOcorrenciaRepository ocorrenciaRepository)
        {
            _distributedCache = distributedCache;
            _ocorrenciaRepository = ocorrenciaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetCachedListAsync()
        {
            string cacheKey = OcorrenciaCacheKeys.ListKey;
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<Ocorrencia>> GetByCategoriaIdCachedListAsync(int categoriaId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromCategoriaKey(categoriaId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromCategoriaAsync(categoriaId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<Ocorrencia>> GetByTipoOcorrenciaIdCachedListAsync(int tipoOcorrenciaId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromTipoOcorrenciaKey(tipoOcorrenciaId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromTipoOcorrenciaAsync(tipoOcorrenciaId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<Ocorrencia>> GetByStatusIdCachedListAsync(int statusId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromStatusKey(statusId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromStatusAsync(statusId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<Ocorrencia>> GetByLojaIdCachedListAsync(int lojaId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromLojaKey(lojaId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromLojaAsync(lojaId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetByMercadoIdCachedListAsync(int? mercadoId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromMercadoKey((int) mercadoId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromMercadoAsync(mercadoId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetByEmpresaIdCachedListAsync(int empresaId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromEmpresaKey(empresaId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromEmpresaAsync(empresaId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetByGrupolojaIdCachedListAsync(int grupolojaId)
        {
            string cacheKey = OcorrenciaCacheKeys.ListFromGrupolojaKey(grupolojaId);
            var ocorrenciaList = await _distributedCache.GetAsync<List<Ocorrencia>>(cacheKey);
            if (ocorrenciaList == null)
            {
                ocorrenciaList = await _ocorrenciaRepository.GetListFromGrupolojaAsync(grupolojaId);
                await _distributedCache.SetAsync(cacheKey, ocorrenciaList);
            }
            return ocorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Ocorrencia> GetByIdAsync(int ocorrenciaId)
        {
            string cacheKey = OcorrenciaCacheKeys.GetKey(ocorrenciaId);
            var ocorrencia = await _distributedCache.GetAsync<Ocorrencia>(cacheKey);
            if (ocorrencia == null)
            {
                ocorrencia = await _ocorrenciaRepository.GetByIdAsync(ocorrenciaId);
                if (ocorrencia == null) return null;
                await _distributedCache.SetAsync(cacheKey, ocorrencia);
            }
            return ocorrencia;
        }


        //---------------------------------------------------------------------------------------------------

    }
}