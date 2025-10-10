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

namespace Core.Features.Charts.CountQueries.CountAllClaimsCached
{
    public class CountAllClaimsByLojaIdCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------

        public int lojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountAllClaimsByLojaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountAllClaimsByLojaIdCachedQueryHandler : IRequestHandler<CountAllClaimsByLojaIdCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimhartCache;
        private readonly ILojaCacheRepository _lojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountAllClaimsByLojaIdCachedQueryHandler(IClaimChartCacheRepository claimhartCache, ILojaCacheRepository lojaCache, IMapper mapper)
        {
            _claimhartCache = claimhartCache;
            _lojaCache = lojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountAllClaimsByLojaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimhartCache.CountClaimsByLojaIdAsync(request.lojaId);
            var lc = await _lojaCache.GetByIdAsync(request.lojaId);
            claimCount.Entity = lc.Nome;
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}