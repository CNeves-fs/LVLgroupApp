using Core.Entities.Business;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GrupolojaRepository : IGrupolojaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Grupoloja> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public GrupolojaRepository(IDistributedCache distributedCache, IRepositoryAsync<Grupoloja> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Grupoloja> Gruposlojas => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<Grupoloja> GetByIdAsync(int grupolojaId)
        {
            return await _repository.Entities.Where(g => g.Id == grupolojaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Grupoloja>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task<List<Grupoloja>> GetListFromEmpresaAsync(int empresaId)
        {
            return await _repository.Entities.Where(g => g.EmpresaId == empresaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Grupoloja grupoloja)
        {
            await _repository.AddAsync(grupoloja);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.GetKey(grupoloja.Id));
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.SelectListFromEmpresaKey(grupoloja.Id));
            return grupoloja.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Grupoloja grupoloja)
        {
            await _repository.DeleteAsync(grupoloja);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.GetKey(grupoloja.Id));
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.SelectListFromEmpresaKey(grupoloja.Id));
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Grupoloja grupoloja)
        {
            await _repository.UpdateAsync(grupoloja);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.GetKey(grupoloja.Id));
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(GrupolojaCacheKeys.SelectListFromEmpresaKey(grupoloja.Id));
        }


        //---------------------------------------------------------------------------------------------------

    }
}