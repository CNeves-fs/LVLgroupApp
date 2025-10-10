using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Mercados.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Mercados.Queries.GetById
{
    public class GetMercadoByIdQuery : IRequest<Result<MercadoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public class GetMercadoByIdQueryHandler : IRequestHandler<GetMercadoByIdQuery, Result<MercadoCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IMercadoCacheRepository _mercadoCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetMercadoByIdQueryHandler(IMercadoCacheRepository mercadoCache, IMapper mapper)
            {
                _mercadoCache = mercadoCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<MercadoCachedResponse>> Handle(GetMercadoByIdQuery query, CancellationToken cancellationToken)
            {
                var mercado = await _mercadoCache.GetByIdAsync(query.Id);
                var mappedMercado = _mapper.Map<MercadoCachedResponse>(mercado);
                return Result<MercadoCachedResponse>.Success(mappedMercado);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}