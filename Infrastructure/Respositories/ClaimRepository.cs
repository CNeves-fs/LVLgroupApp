using Core.Entities.Business;
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
    public class ClaimRepository : IClaimRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Claim> _repository;

        private readonly IRepositoryAsync<Loja> _repositoryLoja;

        private readonly IRepositoryAsync<Grupoloja> _repositoryGrupoloja;

        private readonly IRepositoryAsync<Empresa> _repositoryEmpresa;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ClaimRepository(IDistributedCache distributedCache, 
                               IRepositoryAsync<Claim> repository,
                               IRepositoryAsync<Loja> repositoryLoja, 
                               IRepositoryAsync<Grupoloja> repositoryGrupoloja,
                               IRepositoryAsync<Empresa> repositoryEmpresa)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _repositoryLoja = repositoryLoja;
            _repositoryGrupoloja = repositoryGrupoloja;
            _repositoryEmpresa = repositoryEmpresa;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Claim> Claims => _repository.Entities;

        public IQueryable<Loja> Lojas => _repositoryLoja.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<Claim> GetByIdAsync(int claimId)
        {
            return await _repository.Entities.Where(r => r.Id == claimId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetListFromEmpresaAsync(int empresaId)
        {
            return await _repository.Entities.Where(r => r.EmpresaId == empresaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetListFromGrupolojaAsync(int grupolojaId)
        {
            return await _repository.Entities.Where(r => r.GrupolojaId == grupolojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetListFromLojaAsync(int lojaId)
        {
            return await _repository.Entities.Where(r => r.LojaId == lojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Claim claim)
        {
            await _repository.AddAsync(claim);
            var loja = await _repositoryLoja.Entities.Where(l => l.Id == claim.LojaId).FirstOrDefaultAsync();
            var grupoloja = await _repositoryGrupoloja.Entities.Where(g => g.Id == loja.GrupolojaId).FirstOrDefaultAsync();

            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ClaimCacheKeys.GetKey(claim.Id));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromLojaKey(claim.LojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromLojaKey(claim.LojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromEmpresaKey(grupoloja.EmpresaId));

            return claim.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Claim claim)
        {
            await _repository.DeleteAsync(claim);
            var loja = await _repositoryLoja.Entities.Where(l => l.Id == claim.LojaId).FirstOrDefaultAsync();
            var grupoloja = await _repositoryGrupoloja.Entities.Where(g => g.Id == loja.GrupolojaId).FirstOrDefaultAsync();
            
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ClaimCacheKeys.GetKey(claim.Id));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromLojaKey(claim.LojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromLojaKey(claim.LojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromEmpresaKey(grupoloja.EmpresaId));
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Claim claim)
        {
            await _repository.UpdateAsync(claim);
            var loja = await _repositoryLoja.Entities.Where(l => l.Id == claim.LojaId).FirstOrDefaultAsync();
            var grupoloja = await _repositoryGrupoloja.Entities.Where(g => g.Id == loja.GrupolojaId).FirstOrDefaultAsync();

            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ClaimCacheKeys.GetKey(claim.Id));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromLojaKey(claim.LojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromLojaKey(claim.LojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(ClaimCacheKeys.SelectListFromEmpresaKey(grupoloja.EmpresaId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}