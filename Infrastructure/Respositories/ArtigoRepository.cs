using Core.Entities.Artigos;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ArtigoRepository : IArtigoRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Artigo> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ArtigoRepository(IDistributedCache distributedCache, IRepositoryAsync<Artigo> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Artigo> Artigos => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Artigo artigo)
        {
            await _repository.DeleteAsync(artigo);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.GetKey(artigo.Id));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.GetReferenciaKey(artigo.Referencia));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.ListFromEmpresaKey(artigo.EmpresaId));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.SelectListFromEmpresaKey(artigo.EmpresaId));
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Artigo> GetByIdAsync(int artigoId)
        {
            return await _repository.Entities.Where(a => a.Id == artigoId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Artigo> GetByReferenciaAsync(string referencia)
        {
            return await _repository.Entities.Where(a => a.Referencia == referencia).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Artigo>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Artigo>> GetListFromEmpresaAsync(int empresaId)
        {
            return await _repository.Entities.Where(a => a.EmpresaId == empresaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Artigo artigo)
        {
            await _repository.AddAsync(artigo);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.GetKey(artigo.Id));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.GetReferenciaKey(artigo.Referencia));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.ListFromEmpresaKey(artigo.EmpresaId));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.SelectListFromEmpresaKey(artigo.EmpresaId));
            return artigo.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Artigo artigo)
        {
            await _repository.UpdateAsync(artigo);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.GetKey(artigo.Id));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.GetReferenciaKey(artigo.Referencia));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.ListFromEmpresaKey(artigo.EmpresaId));
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ArtigoCacheKeys.SelectListFromEmpresaKey(artigo.EmpresaId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}