using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Charts;
using Core.Interfaces.ChartCacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Charts.CountQueries.CountClaimsStatusCached
{
    public class CountClaimsAguardaValidacaoCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountClaimsAguardaValidacaoCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountClaimsAguardaValidacaoCachedQueryHandler : IRequestHandler<CountClaimsAguardaValidacaoCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountClaimsAguardaValidacaoCachedQueryHandler(IClaimChartCacheRepository claimChartCache, IMapper mapper)
        {
            _claimChartCache = claimChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountClaimsAguardaValidacaoCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimChartCache.CountClaimsAguardaValidCachedAsync(request.empresaId);
            claimCount.Entity = "";
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}