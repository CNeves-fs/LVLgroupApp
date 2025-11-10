using Core.Entities.Business;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LojaRepository : ILojaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Loja> _repository;

        private readonly IRepositoryAsync<Grupoloja> _repositoryGrupoloja;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public LojaRepository(IDistributedCache distributedCache, IRepositoryAsync<Loja> repository, IRepositoryAsync<Grupoloja> repositoryGrupoloja)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _repositoryGrupoloja = repositoryGrupoloja;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Loja> Lojas => _repository.Entities;

        public IQueryable<Grupoloja> Gruposlojas => _repositoryGrupoloja.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<Loja> GetByIdAsync(int lojaId)
        {
            return await _repository.Entities.Where(l => l.Id == lojaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Loja> GetByNomeAsync(string nome)
        {
            return await _repository.Entities.Where(l => l.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetAllLojasListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetListAsync()
        {
            return await _repository.Entities.Where(l => l.Active == true).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetListFromGrupolojaAsync(int grupolojaId)
        {
            return await _repository.Entities.Where(l => (l.GrupolojaId == grupolojaId) && (l.Active == true)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task<List<Loja>> GetListFromEmpresaAsync(int empresaId)
        {
            var lojasList = new List<Loja>();
            var gruposlojas = await _repositoryGrupoloja.Entities.Where(g => g.EmpresaId == empresaId).ToListAsync();
            foreach (var grupoloja in gruposlojas)
            {
                var tempList = await _repository.Entities.Where(l => (l.GrupolojaId == grupoloja.Id) && (l.Active == true)).ToListAsync();
                lojasList.AddRange(tempList);
            }

            return lojasList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Loja>> GetListFromMercadoAsync(int? mercadoId)
        {
            if (mercadoId == null) return null;
            return await _repository.Entities.Where(l => (l.MercadoId == mercadoId) && (l.Active == true)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Loja loja)
        {
            await _repository.AddAsync(loja);
            var grupoloja = await _repositoryGrupoloja.GetByIdAsync(loja.GrupolojaId);
            await _distributedCache.RemoveAsync(LojaCacheKeys.AllLojasListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromMercadoKey(loja.MercadoId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.GetKey(loja.Id));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromMercadoKey(loja.MercadoId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromEmpresaKey(grupoloja.EmpresaId));
            return loja.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Loja loja)
        {
            await _repository.DeleteAsync(loja);
            var grupoloja = await _repositoryGrupoloja.GetByIdAsync(loja.GrupolojaId);
            await _distributedCache.RemoveAsync(LojaCacheKeys.AllLojasListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromMercadoKey(loja.MercadoId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.GetKey(loja.Id));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromMercadoKey(loja.MercadoId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromEmpresaKey(grupoloja.EmpresaId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(Loja loja)
        {
            await _repository.UpdateAsync(loja);
            var grupoloja = await _repositoryGrupoloja.GetByIdAsync(loja.GrupolojaId);
            await _distributedCache.RemoveAsync(LojaCacheKeys.AllLojasListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromMercadoKey(loja.MercadoId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.ListFromEmpresaKey(grupoloja.EmpresaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.GetKey(loja.Id));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromGrupolojaKey(loja.GrupolojaId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromMercadoKey(loja.MercadoId));
            await _distributedCache.RemoveAsync(LojaCacheKeys.SelectListFromEmpresaKey(grupoloja.EmpresaId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}