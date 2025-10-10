using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Charts;
using Core.Entities.Claims;
using Core.Enums;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.ChartCacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.ChartCacheRepositories
{
    public class ClaimChartCacheRepository : IClaimChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IClaimRepository _claimRepository;

        private readonly IStatusCacheRepository _statusCacheRepository;


        //---------------------------------------------------------------------------------------------------


        public ClaimChartCacheRepository(IDistributedCache distributedCache, IClaimRepository claimRepository, IStatusCacheRepository statusCacheRepository)
        {
            _distributedCache = distributedCache;
            _claimRepository = claimRepository;
            _statusCacheRepository = statusCacheRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountAllClaimsCachedAsync()
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = claimList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsByLojaIdAsync(int lojaId)
        {
            string cacheKey = ClaimCacheKeys.ListFromLojaKey(lojaId);
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListFromLojaAsync(lojaId);
                await _distributedCache.SetAsync(cacheKey, claimList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = claimList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsByGrupolojaIdAsync(int grupolojaId)
        {
            string cacheKey = ClaimCacheKeys.ListFromGrupolojaKey(grupolojaId);
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListFromGrupolojaAsync(grupolojaId);
                //await _distributedCache.SetAsync(cacheKey, claimList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = claimList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsByEmpresaIdAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListFromEmpresaKey(empresaId);
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListFromEmpresaAsync(empresaId);
                //await _distributedCache.SetAsync(cacheKey, claimList);
            }
            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = claimList.Count
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsPendentesCachedAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var pendentes = from claim in claimList
                            join s in statusList on claim.StatusId equals s.Id into slist
                            from sts in slist.DefaultIfEmpty()
                            select new
                            {
                                Id = claim.Id,
                                StatusId = claim.StatusId,
                                Tipo = sts.Tipo,
                                EmpresaId = claim.EmpresaId
                            };
            pendentes = pendentes.Where(t => (t.Tipo == (int)StatusType.PendenteEmLoja) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = pendentes.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsAguardaValidCachedAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var aValidacao = from claim in claimList
                             join s in statusList on claim.StatusId equals s.Id into slist
                             from sts in slist.DefaultIfEmpty()
                             select new
                             {
                                 Id = claim.Id,
                                 StatusId = claim.StatusId,
                                 Tipo = sts.Tipo,
                                 EmpresaId = claim.EmpresaId
                             };
            aValidacao = aValidacao.Where(t => (t.Tipo == (int)StatusType.AguardaValidação) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = aValidacao.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsAguardaDeciCachedAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var aguardaDeci = from claim in claimList
                              join s in statusList on claim.StatusId equals s.Id into slist
                              from sts in slist.DefaultIfEmpty()
                              select new
                              {
                                  Id = claim.Id,
                                  StatusId = claim.StatusId,
                                  Tipo = sts.Tipo,
                                  EmpresaId = claim.EmpresaId
                              };
            aguardaDeci = aguardaDeci.Where(t => (t.Tipo == (int)StatusType.AguardaDecisão) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = aguardaDeci.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsAguardaOpiniaoCachedAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var aOpiniao = from claim in claimList
                           join s in statusList on claim.StatusId equals s.Id into slist
                           from sts in slist.DefaultIfEmpty()
                           select new
                           {
                               Id = claim.Id,
                               StatusId = claim.StatusId,
                               Tipo = sts.Tipo,
                               EmpresaId = claim.EmpresaId
                           };
            aOpiniao = aOpiniao.Where(t => (t.Tipo > (int)StatusType.AguardaDecisão) && (t.Tipo < (int)StatusType.Aceite) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = aOpiniao.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsAceiteCachedAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var aceite = from claim in claimList
                         join s in statusList on claim.StatusId equals s.Id into slist
                         from sts in slist.DefaultIfEmpty()
                         select new
                         {
                             Id = claim.Id,
                             StatusId = claim.StatusId,
                             Tipo = sts.Tipo,
                             EmpresaId = claim.EmpresaId
                         };
            aceite = aceite.Where(t => (t.Tipo == (int)StatusType.Aceite) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = aceite.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsNaoAceiteCachedAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var naoAceite = from claim in claimList
                            join s in statusList on claim.StatusId equals s.Id into slist
                            from sts in slist.DefaultIfEmpty()
                            select new
                            {
                                Id = claim.Id,
                                StatusId = claim.StatusId,
                                Tipo = sts.Tipo,
                                EmpresaId = claim.EmpresaId
                            };
            naoAceite = naoAceite.Where(t => (t.Tipo == (int)StatusType.NãoAceite) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = naoAceite.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsFechadasCachedQuery(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var fechada = from claim in claimList
                          join s in statusList on claim.StatusId equals s.Id into slist
                          from sts in slist.DefaultIfEmpty()
                          select new
                          {
                              Id = claim.Id,
                              StatusId = claim.StatusId,
                              Tipo = sts.Tipo,
                              EmpresaId = claim.EmpresaId
                          };
            fechada = fechada.Where(t => (t.Tipo > (int)StatusType.NãoAceite) && (t.EmpresaId == empresaId));

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = fechada.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsPorDecidirCachedAsync()
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var porDecidir = from claim in claimList
                             join s in statusList on claim.StatusId equals s.Id into slist
                             from sts in slist.DefaultIfEmpty()
                             select new
                             {
                                 Id = claim.Id,
                                 StatusId = claim.StatusId,
                                 Tipo = sts.Tipo,
                                 EmpresaId = claim.EmpresaId
                             };
            porDecidir = porDecidir.Where(t => t.Tipo < (int)StatusType.Aceite);

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = porDecidir.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ChartPoint> CountClaimsPorFecharCachedAsync()
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }

            var statusList = await _statusCacheRepository.GetCachedListAsync();

            var porFechar = from claim in claimList
                            join s in statusList on claim.StatusId equals s.Id into slist
                            from sts in slist.DefaultIfEmpty()
                            select new
                            {
                                Id = claim.Id,
                                StatusId = claim.StatusId,
                                Tipo = sts.Tipo,
                                EmpresaId = claim.EmpresaId
                            };
            porFechar = porFechar.Where(t => t.Tipo < (int)StatusType.FechadaEmLojaRejeitada);

            var cp = new ChartPoint()
            {
                Entity = "",
                EntityCount = porFechar.Count()
            };
            return cp;
        }


        //---------------------------------------------------------------------------------------------------

    }
}