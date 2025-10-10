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
    public class CountClaimsPorDecidirCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountClaimsPorDecidirCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountClaimsPorDecidirCachedQueryHandler : IRequestHandler<CountClaimsPorDecidirCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountClaimsPorDecidirCachedQueryHandler(IClaimChartCacheRepository claimChartCache, IMapper mapper)
        {
            _claimChartCache = claimChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountClaimsPorDecidirCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimChartCache.CountClaimsPorDecidirCachedAsync();
            claimCount.Entity = "";
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}