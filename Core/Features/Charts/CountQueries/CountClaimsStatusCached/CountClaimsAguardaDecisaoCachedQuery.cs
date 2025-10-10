using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Charts;
using Core.Features.Claims.Response;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.ChartCacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Charts.CountQueries.CountClaimsStatusCached
{
    public class CountClaimsAguardaDecisaoCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountClaimsAguardaDecisaoCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountClaimsAguardaDecisaoCachedQueryHandler : IRequestHandler<CountClaimsAguardaDecisaoCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountClaimsAguardaDecisaoCachedQueryHandler(IClaimChartCacheRepository claimChartCache, IMapper mapper)
        {
            _claimChartCache = claimChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountClaimsAguardaDecisaoCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimChartCache.CountClaimsAguardaDeciCachedAsync(request.empresaId);
            claimCount.Entity = "";
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}