using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Charts;
using Core.Interfaces.ChartCacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Charts.CountQueries.CountClaimsStatusCached
{
    public class CountClaimsNaoAceiteCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountClaimsNaoAceiteCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountClaimsNaoAceiteCachedQueryHandler : IRequestHandler<CountClaimsNaoAceiteCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountClaimsNaoAceiteCachedQueryHandler(IClaimChartCacheRepository claimChartCache, IMapper mapper)
        {
            _claimChartCache = claimChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountClaimsNaoAceiteCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimChartCache.CountClaimsNaoAceiteCachedAsync(request.empresaId);
            claimCount.Entity = "";
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}