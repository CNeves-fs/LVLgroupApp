using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Business;
using Core.Entities.Vendas;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Constants.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.CacheRepositories
{
    public class VendaDiariaCacheRepository : IVendaDiariaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IVendaDiariaRepository _vendaDiariaRepository;

        private readonly IVendaSemanalRepository _vendaSemanalRepository;


        //---------------------------------------------------------------------------------------------------


        public VendaDiariaCacheRepository(IDistributedCache distributedCache, IVendaDiariaRepository vendaDiariaRepository, IVendaSemanalRepository vendaSemanalRepository)
        {
            _distributedCache = distributedCache;
            _vendaDiariaRepository = vendaDiariaRepository;
            _vendaSemanalRepository = vendaSemanalRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetCachedListAsync()
        {
            string cacheKey = VendaDiariaCacheKeys.ListKey;
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByLojaIdAsync(int lojaId)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromLojaKey(lojaId);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromLojaIdAsync(lojaId);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByLojaIdAnoAsync(int lojaId, int ano)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromLojaAnoKey(lojaId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByLojaIdAnoAsync(lojaId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByLojaIdQuarterAsync(int lojaId, int ano, int quarter)
        {

            var vendaDiariaList = new List<VendaDiaria>();

            var month1_OfQuarter = (quarter * 3) - 2;
            var month2_OfQuarter = month1_OfQuarter + 1;
            var month3_OfQuarter = month2_OfQuarter + 1;

            string cacheKey1 = VendaDiariaCacheKeys.ListFromLojaMesKey(lojaId, ano, month1_OfQuarter);
            string cacheKey2 = VendaDiariaCacheKeys.ListFromLojaMesKey(lojaId, ano, month2_OfQuarter);
            string cacheKey3 = VendaDiariaCacheKeys.ListFromLojaMesKey(lojaId, ano, month3_OfQuarter);

            var vendaDiariaList1 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey1);
            if (vendaDiariaList1 == null)
            {
                vendaDiariaList1 = await _vendaDiariaRepository.GetListByLojaIdMesAsync(lojaId, ano, month1_OfQuarter);
                if (vendaDiariaList1 != null)
                {
                    await _distributedCache.SetAsync(cacheKey1, vendaDiariaList1);
                    vendaDiariaList.AddRange(vendaDiariaList1);
                }
            }
            var vendaDiariaList2 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey2);
            if (vendaDiariaList2 == null)
            {
                vendaDiariaList2 = await _vendaDiariaRepository.GetListByLojaIdMesAsync(lojaId, ano, month2_OfQuarter);
                if (vendaDiariaList2 != null)
                {
                    await _distributedCache.SetAsync(cacheKey2, vendaDiariaList2);
                    vendaDiariaList.AddRange(vendaDiariaList2);
                }
            }
            var vendaDiariaList3 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey3);
            if (vendaDiariaList3 == null)
            {
                vendaDiariaList3 = await _vendaDiariaRepository.GetListByLojaIdMesAsync(lojaId, ano, month3_OfQuarter);
                if (vendaDiariaList3 != null)
                {
                    await _distributedCache.SetAsync(cacheKey3, vendaDiariaList3);
                    vendaDiariaList.AddRange(vendaDiariaList3);
                }
            }

            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByLojaIdMesAsync(int lojaId, int ano, int mês)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromLojaMesKey(lojaId, ano, mês);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByLojaIdMesAsync(lojaId, ano, mês);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaDiaria> GetByLojaIdDataAsync(int lojaId, int ano, int mês, int diaDoMês)
        {
            string cacheKey = VendaDiariaCacheKeys.GetKeyFromLojaData(lojaId, ano, mês, diaDoMês);
            var vendaDiaria = await _distributedCache.GetAsync<VendaDiaria>(cacheKey);
            if (vendaDiaria == null)
            {
                vendaDiaria = await _vendaDiariaRepository.GetByLojaIdDataAsync(lojaId, ano, mês, diaDoMês);
                if (vendaDiaria == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiaria);
            }
            return vendaDiaria;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByLojaIdSemanaAsync(int lojaId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromLojaAnoKey(lojaId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            var vendaSemanal = await _vendaSemanalRepository.GetByLojaIdSemanaAsync(lojaId, ano, numeroDaSemana);

            if (vendaSemanal == null) return null;
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByLojaIdAnoAsync(lojaId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }

            vendaDiariaList = vendaDiariaList.Where(v => v.VendaSemanalId == vendaSemanal.Id).ToList();
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByMercadoIdAsync(int mercadoId)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromMercadoKey(mercadoId);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByMercadoIdCachedListAsync(mercadoId);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByMercadoIdAnoAsync(int mercadoId, int ano)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromMercadoAnoKey(mercadoId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByMercadoIdAnoAsync(mercadoId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByMercadoIdQuarterAsync(int mercadoId, int ano, int quarter)
        {
            var vendaDiariaList = new List<VendaDiaria>();

            var month1_OfQuarter = (quarter * 3) - 2;
            var month2_OfQuarter = month1_OfQuarter + 1;
            var month3_OfQuarter = month2_OfQuarter + 1;

            string cacheKey1 = VendaDiariaCacheKeys.ListFromMercadoMesKey(mercadoId, ano, month1_OfQuarter);
            string cacheKey2 = VendaDiariaCacheKeys.ListFromMercadoMesKey(mercadoId, ano, month2_OfQuarter);
            string cacheKey3 = VendaDiariaCacheKeys.ListFromMercadoMesKey(mercadoId, ano, month3_OfQuarter);

            var vendaDiariaList1 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey1);
            if (vendaDiariaList1 == null)
            {
                vendaDiariaList1 = await _vendaDiariaRepository.GetListByMercadoIdMesAsync(mercadoId, ano, month1_OfQuarter);
                if (vendaDiariaList1 != null)
                {
                    await _distributedCache.SetAsync(cacheKey1, vendaDiariaList1);
                    vendaDiariaList.AddRange(vendaDiariaList1);
                }
            }
            var vendaDiariaList2 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey2);
            if (vendaDiariaList2 == null)
            {
                vendaDiariaList2 = await _vendaDiariaRepository.GetListByMercadoIdMesAsync(mercadoId, ano, month2_OfQuarter);
                if (vendaDiariaList2 != null)
                {
                    await _distributedCache.SetAsync(cacheKey2, vendaDiariaList2);
                    vendaDiariaList.AddRange(vendaDiariaList2);
                }
            }
            var vendaDiariaList3 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey3);
            if (vendaDiariaList3 == null)
            {
                vendaDiariaList3 = await _vendaDiariaRepository.GetListByMercadoIdMesAsync(mercadoId, ano, month3_OfQuarter);
                if (vendaDiariaList3 != null)
                {
                    await _distributedCache.SetAsync(cacheKey3, vendaDiariaList3);
                    vendaDiariaList.AddRange(vendaDiariaList3);
                }
            }

            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByMercadoIdMesAsync(int mercadoId, int ano, int mes)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromMercadoMesKey(mercadoId, ano, mes);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByMercadoIdMesAsync(mercadoId, ano, mes);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByMercadoIdSemanaAsync(int mercadoId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromMercadoAnoKey(mercadoId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            var vendaSemanalList = await _vendaSemanalRepository.GetListByMercadoIdSemanaAsync(mercadoId, ano, numeroDaSemana);
            var listIds = vendaSemanalList.Select(vs => vs.Id).ToList();

            if (vendaSemanalList == null) return null;
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByMercadoIdAnoAsync(mercadoId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }

            // Filtrar vendaDiariaList by VendaSemanalId
            vendaDiariaList = vendaDiariaList.Where(v => listIds.Contains(v.VendaSemanalId)).ToList();
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByEmpresaIdAsync(int empresaId)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromEmpresaKey(empresaId);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByEmpresaIdCachedListAsync(empresaId);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByEmpresaIdAnoAsync(int empresaId, int ano)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromEmpresaAnoKey(empresaId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByEmpresaIdAnoAsync(empresaId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByEmpresaIdQuarterAsync(int empresaId, int ano, int quarter)
        {
            var vendaDiariaList = new List<VendaDiaria>();

            var month1_OfQuarter = (quarter * 3) - 2;
            var month2_OfQuarter = month1_OfQuarter + 1;
            var month3_OfQuarter = month2_OfQuarter + 1;

            string cacheKey1 = VendaDiariaCacheKeys.ListFromEmpresaMesKey(empresaId, ano, month1_OfQuarter);
            string cacheKey2 = VendaDiariaCacheKeys.ListFromEmpresaMesKey(empresaId, ano, month2_OfQuarter);
            string cacheKey3 = VendaDiariaCacheKeys.ListFromEmpresaMesKey(empresaId, ano, month3_OfQuarter);

            var vendaDiariaList1 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey1);
            if (vendaDiariaList1 == null)
            {
                vendaDiariaList1 = await _vendaDiariaRepository.GetListByEmpresaIdMesAsync(empresaId, ano, month1_OfQuarter);
                if (vendaDiariaList1 != null)
                {
                    await _distributedCache.SetAsync(cacheKey1, vendaDiariaList1);
                    vendaDiariaList.AddRange(vendaDiariaList1);
                }
            }
            var vendaDiariaList2 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey2);
            if (vendaDiariaList2 == null)
            {
                vendaDiariaList2 = await _vendaDiariaRepository.GetListByEmpresaIdMesAsync(empresaId, ano, month2_OfQuarter);
                if (vendaDiariaList2 != null)
                {
                    await _distributedCache.SetAsync(cacheKey2, vendaDiariaList2);
                    vendaDiariaList.AddRange(vendaDiariaList2);
                }
            }
            var vendaDiariaList3 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey3);
            if (vendaDiariaList3 == null)
            {
                vendaDiariaList3 = await _vendaDiariaRepository.GetListByEmpresaIdMesAsync(empresaId, ano, month3_OfQuarter);
                if (vendaDiariaList3 != null)
                {
                    await _distributedCache.SetAsync(cacheKey3, vendaDiariaList3);
                    vendaDiariaList.AddRange(vendaDiariaList3);
                }
            }

            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByEmpresaIdMesAsync(int empresaId, int ano, int mes)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromEmpresaMesKey(empresaId, ano, mes);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByEmpresaIdMesAsync(empresaId, ano, mes);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByEmpresaIdSemanaAsync(int empresaId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromEmpresaAnoKey(empresaId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            var vendaSemanalList = await _vendaSemanalRepository.GetListByEmpresaIdSemanaAsync(empresaId, ano, numeroDaSemana);
            var listIds = vendaSemanalList.Select(vs => vs.Id).ToList();

            if (vendaSemanalList == null) return null;
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByEmpresaIdAnoAsync(empresaId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }

            // Filtrar vendaDiariaList by VendaSemanalId
            vendaDiariaList = vendaDiariaList.Where(v => listIds.Contains(v.VendaSemanalId)).ToList();
            return vendaDiariaList;
        }

        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByGrupolojaIdAsync(int grupolojaId)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromGrupolojaKey(grupolojaId);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByGrupolojaIdCachedListAsync(grupolojaId);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByGrupolojaIdAnoAsync(int grupolojaId, int ano)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromGrupolojaAnoKey(grupolojaId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByGrupolojaIdAnoAsync(grupolojaId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByGrupolojaIdQuarterAsync(int grupolojaId, int ano, int quarter)
        {
            var vendaDiariaList = new List<VendaDiaria>();

            var month1_OfQuarter = (quarter * 3) - 2;
            var month2_OfQuarter = month1_OfQuarter + 1;
            var month3_OfQuarter = month2_OfQuarter + 1;

            string cacheKey1 = VendaDiariaCacheKeys.ListFromGrupolojaMesKey(grupolojaId, ano, month1_OfQuarter);
            string cacheKey2 = VendaDiariaCacheKeys.ListFromGrupolojaMesKey(grupolojaId, ano, month2_OfQuarter);
            string cacheKey3 = VendaDiariaCacheKeys.ListFromGrupolojaMesKey(grupolojaId, ano, month3_OfQuarter);

            var vendaDiariaList1 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey1);
            if (vendaDiariaList1 == null)
            {
                vendaDiariaList1 = await _vendaDiariaRepository.GetListByGrupolojaIdMesAsync(grupolojaId, ano, month1_OfQuarter);
                if (vendaDiariaList1 != null)
                {
                    await _distributedCache.SetAsync(cacheKey1, vendaDiariaList1);
                    vendaDiariaList.AddRange(vendaDiariaList1);
                }
            }
            var vendaDiariaList2 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey2);
            if (vendaDiariaList2 == null)
            {
                vendaDiariaList2 = await _vendaDiariaRepository.GetListByGrupolojaIdMesAsync(grupolojaId, ano, month2_OfQuarter);
                if (vendaDiariaList2 != null)
                {
                    await _distributedCache.SetAsync(cacheKey2, vendaDiariaList2);
                    vendaDiariaList.AddRange(vendaDiariaList2);
                }
            }
            var vendaDiariaList3 = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey3);
            if (vendaDiariaList3 == null)
            {
                vendaDiariaList3 = await _vendaDiariaRepository.GetListByGrupolojaIdMesAsync(grupolojaId, ano, month3_OfQuarter);
                if (vendaDiariaList3 != null)
                {
                    await _distributedCache.SetAsync(cacheKey3, vendaDiariaList3);
                    vendaDiariaList.AddRange(vendaDiariaList3);
                }
            }

            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByGrupolojaIdMesAsync(int grupolojaId, int ano, int mes)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromGrupolojaMesKey(grupolojaId, ano, mes);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByGrupolojaIdMesAsync(grupolojaId, ano, mes);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByGrupolojaIdSemanaAsync(int grupolojaId, int ano, int numeroDaSemana)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromGrupolojaAnoKey(grupolojaId, ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            var vendaSemanalList = await _vendaSemanalRepository.GetListByGrupolojaIdSemanaAsync(grupolojaId, ano, numeroDaSemana);
            var listIds = vendaSemanalList.Select(vs => vs.Id).ToList();

            if (vendaSemanalList == null) return null;
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListByGrupolojaIdAnoAsync(grupolojaId, ano);
                if (vendaDiariaList == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }

            // Filtrar vendaDiariaList by VendaSemanalId
            vendaDiariaList = vendaDiariaList.Where(v => listIds.Contains(v.VendaSemanalId)).ToList();
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByAnoAsync(int ano)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromAnoKey(ano);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromAnoAsync(ano);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByTrimestreAsync(int ano, int trimestre)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromTrimestreKey(ano, trimestre);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromTrimestreAsync(ano, trimestre);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByMesAsync(int ano, int mes)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromMesKey(ano, mes);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromMesAsync(ano, mes);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByDiaAsync(int ano, int mes, int dia)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromDiaKey(ano, mes, dia);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromDiaAsync(ano, mes, dia);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetBySemanaAsync(int ano, int semana)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromSemanaKey(ano, semana);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromSemanaAsync(ano, semana);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<VendaDiaria>> GetByVendaSemanalIdAsync(int vendaSemanalId)
        {
            string cacheKey = VendaDiariaCacheKeys.ListFromVendaSemanalKey(vendaSemanalId);
            var vendaDiariaList = await _distributedCache.GetAsync<List<VendaDiaria>>(cacheKey);
            if (vendaDiariaList == null)
            {
                vendaDiariaList = await _vendaDiariaRepository.GetListFromVendaSemanalIdAsync(vendaSemanalId);
                await _distributedCache.SetAsync(cacheKey, vendaDiariaList);
            }
            return vendaDiariaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaDiaria> GetByVendaSemanalIdDiaAsync(int vendaSemanalId, int diaDaSemana)
        {
            string cacheKey = VendaDiariaCacheKeys.GetKeyFromVendaSemanalDia(vendaSemanalId, diaDaSemana);
            var vendaDiaria = await _distributedCache.GetAsync<VendaDiaria>(cacheKey);
            if (vendaDiaria == null)
            {
                vendaDiaria = await _vendaDiariaRepository.GetByVendaSemanalIdDiaAsync(vendaSemanalId, diaDaSemana);
                if (vendaDiaria == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiaria);
            }
            return vendaDiaria;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<VendaDiaria> GetByIdAsync(int vendaDiariaId)
        {
            string cacheKey = VendaDiariaCacheKeys.GetKey(vendaDiariaId);
            var vendaDiaria = await _distributedCache.GetAsync<VendaDiaria>(cacheKey);
            if (vendaDiaria == null)
            {
                vendaDiaria = await _vendaDiariaRepository.GetByIdAsync(vendaDiariaId);
                if (vendaDiaria == null) return null;
                await _distributedCache.SetAsync(cacheKey, vendaDiaria);
            }
            return vendaDiaria;
        }


        //---------------------------------------------------------------------------------------------------

    }
}