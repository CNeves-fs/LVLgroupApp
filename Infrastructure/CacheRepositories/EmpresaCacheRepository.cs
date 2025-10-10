using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Infrastructure.CacheRepositories
{
    public class EmpresaCacheRepository : IEmpresaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IEmpresaRepository _empresaRepository;


        //---------------------------------------------------------------------------------------------------


        public EmpresaCacheRepository(IDistributedCache distributedCache, IEmpresaRepository empresaRepository)
        {
            _distributedCache = distributedCache;
            _empresaRepository = empresaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Empresa> GetByIdAsync(int empresaId)
        {
            string cacheKey = EmpresaCacheKeys.GetKey(empresaId);
            var empresa = await _distributedCache.GetAsync<Empresa>(cacheKey);
            if (empresa == null)
            {
                empresa = await _empresaRepository.GetByIdAsync(empresaId);
                // Throw.Exception.IfNull(empresa, "Empresa", "Empresa not Found");
                if (empresa == null) return null;
                await _distributedCache.SetAsync(cacheKey, empresa);
            }
            return empresa;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Empresa> GetByNomeAsync(string nome)
        {
            string cacheKey = EmpresaCacheKeys.GetNomeKey(nome);
            var empresa = await _distributedCache.GetAsync<Empresa>(cacheKey);
            if (empresa == null)
            {
                empresa = await _empresaRepository.GetByNomeAsync(nome);
                //Throw.Exception.IfNull(empresa, "Empresa", "Empresa not Found");
                if (empresa == null) return null;
                await _distributedCache.SetAsync(cacheKey, empresa);
            }
            return empresa;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Empresa>> GetCachedListAsync()
        {
            string cacheKey = EmpresaCacheKeys.ListKey;
            var empresaList = await _distributedCache.GetAsync<List<Empresa>>(cacheKey);
            if (empresaList == null)
            {
                empresaList = await _empresaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, empresaList);
            }
            return empresaList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}