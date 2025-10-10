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
    public class OcorrenciaRepository : IOcorrenciaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Ocorrencia> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public OcorrenciaRepository(IDistributedCache distributedCache, IRepositoryAsync<Ocorrencia> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Ocorrencia> Ocorrencias => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromCategoriaAsync(int categoriaId)
        {
            return await _repository.Entities.Where(o => o.CategoriaId == categoriaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromTipoOcorrenciaAsync(int tipoOcorrenciaId)
        {
            return await _repository.Entities.Where(o => o.TipoOcorrenciaId == tipoOcorrenciaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromStatusAsync(int statusId)
        {
            return await _repository.Entities.Where(o => o.StatusId == statusId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromLojaAsync(int lojaId)
        {
            return await _repository.Entities.Where(o => o.LojaId == lojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromMercadoAsync(int? mercadoId)
        {
            return await _repository.Entities.Where(o => (o.MercadoId == mercadoId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromEmpresaAsync(int empresaId)
        {
            return await _repository.Entities.Where(o => o.EmpresaId == empresaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Ocorrencia>> GetListFromGrupolojaAsync(int grupolojaId)
        {
            return await _repository.Entities.Where(o => o.GrupolojaId == grupolojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------





        public async Task<Ocorrencia> GetByIdAsync(int ocorrenciaId)
        {
            return await _repository.Entities.Where(o => o.Id == ocorrenciaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Ocorrencia ocorrencia)
        {
            await _repository.AddAsync(ocorrencia);
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.GetKey(ocorrencia.Id));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromLojaKey(ocorrencia.LojaId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromMercadoKey(ocorrencia.MercadoId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromEmpresaKey(ocorrencia.EmpresaId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromGrupolojaKey(ocorrencia.GrupolojaId));

            return ocorrencia.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Ocorrencia ocorrencia)
        {
            await _repository.DeleteAsync(ocorrencia);
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.GetKey(ocorrencia.Id));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromLojaKey(ocorrencia.LojaId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromMercadoKey(ocorrencia.MercadoId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromEmpresaKey(ocorrencia.EmpresaId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromGrupolojaKey(ocorrencia.GrupolojaId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(Ocorrencia ocorrencia)
        {
            await _repository.UpdateAsync(ocorrencia);
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.GetKey(ocorrencia.Id));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromLojaKey(ocorrencia.LojaId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromMercadoKey(ocorrencia.MercadoId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromEmpresaKey(ocorrencia.EmpresaId));
            await _distributedCache.RemoveAsync(OcorrenciaCacheKeys.ListFromGrupolojaKey(ocorrencia.GrupolojaId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}