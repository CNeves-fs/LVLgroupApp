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
    public class NotificacaoOcorrenciaCacheRepository : INotificacaoOcorrenciaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly INotificacaoOcorrenciaRepository _notificacaoOcorrenciaRepository;


        //---------------------------------------------------------------------------------------------------


        public NotificacaoOcorrenciaCacheRepository(IDistributedCache distributedCache, INotificacaoOcorrenciaRepository notificacaoOcorrenciaRepository)
        {
            _distributedCache = distributedCache;
            _notificacaoOcorrenciaRepository = notificacaoOcorrenciaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificacaoOcorrencia>> GetCachedListAsync()
        {
            string cacheKey = NotificacaoOcorrenciaCacheKeys.ListKey;
            var notificacaoOcorrenciaList = await _distributedCache.GetAsync<List<NotificacaoOcorrencia>>(cacheKey);
            if (notificacaoOcorrenciaList == null)
            {
                notificacaoOcorrenciaList = await _notificacaoOcorrenciaRepository.GetListAsync();
                //await _distributedCache.SetAsync(cacheKey, notificacaoOcorrenciaList);
            }
            return notificacaoOcorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<NotificacaoOcorrencia> GetByIdAsync(int notificacaoOcorrenciaId)
        {
            string cacheKey = NotificacaoOcorrenciaCacheKeys.GetKey(notificacaoOcorrenciaId);
            var notificacaoOcorrencia = await _distributedCache.GetAsync<NotificacaoOcorrencia>(cacheKey);
            if (notificacaoOcorrencia == null)
            {
                notificacaoOcorrencia = await _notificacaoOcorrenciaRepository.GetByIdAsync(notificacaoOcorrenciaId);
                if (notificacaoOcorrencia == null) return null;
                //await _distributedCache.SetAsync(cacheKey, notificacaoOcorrencia);
            }
            return notificacaoOcorrencia;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificacaoOcorrencia>> GetByTipoOcorrenciaIdAsync(int tipoOcorrenciaId)
        {
            string cacheKey = NotificacaoOcorrenciaCacheKeys.ListFromTipoOcorrenciaKey(tipoOcorrenciaId);
            var notificacaoOcorrenciaList = await _distributedCache.GetAsync<List<NotificacaoOcorrencia>>(cacheKey);
            if (notificacaoOcorrenciaList == null)
            {
                notificacaoOcorrenciaList = await _notificacaoOcorrenciaRepository.GetListFromTipoOcorrenciaIdAsync(tipoOcorrenciaId);
                if (notificacaoOcorrenciaList == null) return null;
                //await _distributedCache.SetAsync(cacheKey, notificacaoOcorrenciaList);
            }
            return notificacaoOcorrenciaList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}