using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificacaoOcorrenciaRepository : INotificacaoOcorrenciaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<NotificacaoOcorrencia> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public NotificacaoOcorrenciaRepository(IDistributedCache distributedCache, IRepositoryAsync<NotificacaoOcorrencia> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<NotificacaoOcorrencia> NotificacoesOcorrencia => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificacaoOcorrencia>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<NotificacaoOcorrencia> GetByIdAsync(int notificacaoOcorrenciaId)
        {
            return await _repository.Entities.Where(no => no.Id == notificacaoOcorrenciaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<NotificacaoOcorrencia>> GetListFromTipoOcorrenciaIdAsync(int tipoOcorrenciaId)
        {
            return await _repository.Entities.Where(no => no.TipoOcorrenciaId == tipoOcorrenciaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(NotificacaoOcorrencia notificacaoOcorrencia)
        {
            await _repository.AddAsync(notificacaoOcorrencia);
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.GetKey(notificacaoOcorrencia.Id));
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.ListFromTipoOcorrenciaKey(notificacaoOcorrencia.TipoOcorrenciaId));

            return notificacaoOcorrencia.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(NotificacaoOcorrencia notificacaoOcorrencia)
        {
            await _repository.DeleteAsync(notificacaoOcorrencia);
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.GetKey(notificacaoOcorrencia.Id));
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.ListFromTipoOcorrenciaKey(notificacaoOcorrencia.TipoOcorrenciaId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(NotificacaoOcorrencia notificacaoOcorrencia)
        {
            await _repository.UpdateAsync(notificacaoOcorrencia);
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.GetKey(notificacaoOcorrencia.Id));
            await _distributedCache.RemoveAsync(NotificacaoOcorrenciaCacheKeys.ListFromTipoOcorrenciaKey(notificacaoOcorrencia.TipoOcorrenciaId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}