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

namespace Core.Features.Charts.CountQueries.CountAllLojasCached
{
    public class CountAllLojasCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public CountAllLojasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountAllLojasCachedQueryHandler : IRequestHandler<CountAllLojasCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaChartCacheRepository _lojaChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountAllLojasCachedQueryHandler(ILojaChartCacheRepository lojaChartCache, IMapper mapper)
        {
            _lojaChartCache = lojaChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountAllLojasCachedQuery request, CancellationToken cancellationToken)
        {
            var lojaCount = await _lojaChartCache.CountAllLojasCachedAsync();
            lojaCount.Entity = "";
            return Result<ChartPoint>.Success(lojaCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}