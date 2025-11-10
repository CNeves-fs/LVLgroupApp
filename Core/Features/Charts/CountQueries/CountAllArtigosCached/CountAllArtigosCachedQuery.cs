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

namespace Core.Features.Charts.CountQueries.CountAllArtigosCached
{
    public class CountAllArtigosCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public CountAllArtigosCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountAllArtigosCachedQueryHandler : IRequestHandler<CountAllArtigosCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IArtigoChartCacheRepository _artigoChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountAllArtigosCachedQueryHandler(IArtigoChartCacheRepository artigoChartCache, IMapper mapper)
        {
            _artigoChartCache = artigoChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountAllArtigosCachedQuery request, CancellationToken cancellationToken)
        {
            var artigoCount = await _artigoChartCache.CountAllArtigosCachedAsync();
            artigoCount.Entity = "";
            return Result<ChartPoint>.Success(artigoCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}