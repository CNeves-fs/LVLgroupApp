using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Vendas;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class VendaSemanalCacheRepository : IVendaSemanalCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IVendaSemanalRepository _vendaSemanalRepository;


        //---------------------------------------------------------------------------------------------------


        public VendaSemanalCacheRepository(IDistributedCache distributedCache, IVendaSemanalRepository vendaSemanalRepository)
        {
            _distributedCache = distributedCache;
            _vendaSemanalRepository = vendaSemanalRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetCachedListAsync()
        {
            string cacheKey = VendaSemanalCacheKeys.ListKey;
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByAnoAsync(int ano)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromAnoKey(ano);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByAnoAsync(ano);
                if (vendaSemanalList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetBySemanaAsync(int ano, int numeroDaSemana)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromSemanaKey(ano, numeroDaSemana);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListBySemanaAsync(ano, numeroDaSemana);
                if (vendaSemanalList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<VendaSemanal>> GetByLojaIdCachedListAsync(int lojaId)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromLojaKey(lojaId);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListFromLojaIdAsync(lojaId);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaSemanal> GetByLojaIdSemanaAsync(int lojaId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromLojaSemanaKey(lojaId, ano, numeroDaSemana);
            var vendaSemanal = await _distributedCache.GetAsync<VendaSemanal>(cacheKey);
            if (vendaSemanal == null)
            {
                vendaSemanal = await _vendaSemanalRepository.GetByLojaIdSemanaAsync(lojaId, ano, numeroDaSemana);
                if (vendaSemanal == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanal);
            }
            return vendaSemanal;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByLojaIdMesAsync(int lojaId, int ano, int mes)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromLojaMesKey(lojaId, ano, mes);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByLojaIdMesAsync(lojaId, ano, mes);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByLojaIdQuarterAsync(int lojaId, int ano, int quarter)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromLojaQuarterKey(lojaId, ano, quarter);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByLojaIdQuarterAsync(lojaId, ano, quarter);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByLojaIdAnoAsync(int lojaId, int ano)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromLojaAnoKey(lojaId, ano);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByLojaIdAnoAsync(lojaId, ano);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByMercadoIdCachedListAsync(int mercadoId)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromMercadoKey(mercadoId);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByMercadoIdCachedListAsync(mercadoId);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByMercadoIdSemanaAsync(int mercadoId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromMercadoSemanaKey(mercadoId, ano, numeroDaSemana);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByMercadoIdSemanaAsync(mercadoId, ano, numeroDaSemana);
                if (vendaSemanalList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByMercadoIdMesAsync(int mercadoId, int ano, int mes)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromMercadoMesKey(mercadoId, ano, mes);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByMercadoIdMesAsync(mercadoId, ano, mes);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByMercadoIdQuarterAsync(int mercadoId, int ano, int quarter)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromMercadoQuarterKey(mercadoId, ano, quarter);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByMercadoIdQuarterAsync(mercadoId, ano, quarter);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByMercadoIdAnoAsync(int mercadoId, int ano)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromMercadoAnoKey(mercadoId, ano);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByMercadoIdAnoAsync(mercadoId, ano);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByEmpresaIdCachedListAsync(int empresaId)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromEmpresaKey(empresaId);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByEmpresaIdCachedListAsync(empresaId);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByEmpresaIdSemanaAsync(int empresaId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromEmpresaSemanaKey(empresaId, ano, numeroDaSemana);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByEmpresaIdSemanaAsync(empresaId, ano, numeroDaSemana);
                if (vendaSemanalList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByEmpresaIdMesAsync(int empresaId, int ano, int mes)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromEmpresaMesKey(empresaId, ano, mes);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByEmpresaIdMesAsync(empresaId, ano, mes);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByEmpresaIdQuarterAsync(int empresaId, int ano, int quarter)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromEmpresaQuarterKey(empresaId, ano, quarter);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByEmpresaIdQuarterAsync(empresaId, ano, quarter);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByEmpresaIdAnoAsync(int empresaId, int ano)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromEmpresaAnoKey(empresaId, ano);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByEmpresaIdAnoAsync(empresaId, ano);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByGrupolojaIdCachedListAsync(int grupolojaId)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromGrupolojaKey(grupolojaId);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByGrupolojaIdCachedListAsync(grupolojaId);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByGrupolojaIdSemanaAsync(int grupolojaId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromGrupolojaSemanaKey(grupolojaId, ano, numeroDaSemana);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByGrupolojaIdSemanaAsync(grupolojaId, ano, numeroDaSemana);
                if (vendaSemanalList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromGrupolojaMesKey(grupolojaId, ano, mes);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByGrupolojaIdMesAsync(grupolojaId, ano, mes);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByGrupolojaIdQuarterAsync(int grupolojaId, int ano, int quarter)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromGrupolojaQuarterKey(grupolojaId, ano, quarter);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByGrupolojaIdQuarterAsync(grupolojaId, ano, quarter);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaSemanal>> GetByGrupolojaIdAnoAsync(int grupolojaId, int ano)
        {
            string cacheKey = VendaSemanalCacheKeys.ListFromGrupolojaAnoKey(grupolojaId, ano);
            var vendaSemanalList = await _distributedCache.GetAsync<List<VendaSemanal>>(cacheKey);
            if (vendaSemanalList == null)
            {
                vendaSemanalList = await _vendaSemanalRepository.GetListByGrupolojaIdAnoAsync(grupolojaId, ano);
                await _distributedCache.SetAsync(cacheKey, vendaSemanalList);
            }
            return vendaSemanalList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaSemanal> GetByIdAsync(int vendaSemanalId)
        {
            string cacheKey = VendaSemanalCacheKeys.GetKey(vendaSemanalId);
            var vendaSemanal = await _distributedCache.GetAsync<VendaSemanal>(cacheKey);
            if (vendaSemanal == null)
            {
                vendaSemanal = await _vendaSemanalRepository.GetByIdAsync(vendaSemanalId);
                if (vendaSemanal == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaSemanal);
            }
            return vendaSemanal;
        }


        //---------------------------------------------------------------------------------------------------

    }
}