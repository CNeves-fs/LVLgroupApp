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
    public class EmpresaRepository : IEmpresaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Empresa> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public EmpresaRepository(IDistributedCache distributedCache, IRepositoryAsync<Empresa> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Empresa> Empresas => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Empresa empresa)
        {
            await _repository.DeleteAsync(empresa);
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.GetKey(empresa.Id));
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.GetNomeKey(empresa.Nome));
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Empresa> GetByIdAsync(int empresaId)
        {
            return await _repository.Entities.Where(e => e.Id == empresaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Empresa> GetByNomeAsync(string nome)
        {
            return await _repository.Entities.Where(e => e.Nome == nome).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Empresa>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Empresa empresa)
        {
            await _repository.AddAsync(empresa);
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.GetKey(empresa.Id));
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.GetNomeKey(empresa.Nome));
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.SelectListKey);
            return empresa.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Empresa empresa)
        {
            await _repository.UpdateAsync(empresa);
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.GetKey(empresa.Id));
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.GetNomeKey(empresa.Nome));
            await _distributedCache.RemoveAsync(EmpresaCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------

    }
}