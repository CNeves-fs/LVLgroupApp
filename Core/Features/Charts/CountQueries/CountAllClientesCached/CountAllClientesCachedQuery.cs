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

namespace Core.Features.Charts.CountQueries.CountAllClientesCached
{
    public class CountAllClientesCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        public CountAllClientesCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountAllClientesCachedQueryHandler : IRequestHandler<CountAllClientesCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClienteChartCacheRepository _clienteChartCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountAllClientesCachedQueryHandler(IClienteChartCacheRepository clienteChartCache, IMapper mapper)
        {
            _clienteChartCache = clienteChartCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountAllClientesCachedQuery request, CancellationToken cancellationToken)
        {
            var clienteCount = await _clienteChartCache.CountAllClientesCachedAsync();
            clienteCount.Entity = "";
            return Result<ChartPoint>.Success(clienteCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}