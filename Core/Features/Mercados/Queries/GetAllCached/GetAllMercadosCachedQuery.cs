using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Mercados.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Mercados.Queries.GetAllCached
{
    public class GetAllMercadosCachedQuery : IRequest<Result<List<MercadoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllMercadosCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }

    public class GetAllMercadosCachedQueryHandler : IRequestHandler<GetAllMercadosCachedQuery, Result<List<MercadoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IMercadoCacheRepository _mercadoCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllMercadosCachedQueryHandler(IMercadoCacheRepository mercadoCache, IMapper mapper)
        {
            _mercadoCache = mercadoCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<MercadoCachedResponse>>> Handle(GetAllMercadosCachedQuery request, CancellationToken cancellationToken)
        {
            var mercadoList = await _mercadoCache.GetCachedListAsync();
            var mappedMercados = _mapper.Map<List<MercadoCachedResponse>>(mercadoList);
            return Result<List<MercadoCachedResponse>>.Success(mappedMercados);
        }


        //---------------------------------------------------------------------------------------------------

    }
}