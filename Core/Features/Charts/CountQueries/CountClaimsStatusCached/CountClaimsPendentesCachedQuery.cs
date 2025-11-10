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
    public class CountClaimsPendentesCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountClaimsPendentesCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountClaimsPendentesCachedQueryHandler : IRequestHandler<CountClaimsPendentesCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountClaimsPendentesCachedQueryHandler(IClaimChartCacheRepository claimChartCache, IMapper mapper)
        {
            _claimChartCache = claimChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountClaimsPendentesCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimChartCache.CountClaimsPendentesCachedAsync(request.empresaId);
            claimCount.Entity = "";
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}