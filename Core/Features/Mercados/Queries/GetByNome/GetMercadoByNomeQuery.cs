using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Mercados.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Mercados.Queries.GetByNome
{
    public class GetMercadoByNomeQuery : IRequest<Result<MercadoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Nome { get; set; }


        public class GetMercadoByNomeQueryHandler : IRequestHandler<GetMercadoByNomeQuery, Result<MercadoCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IMercadoCacheRepository _mercadoCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetMercadoByNomeQueryHandler(IMercadoCacheRepository mercadoCache, IMapper mapper)
            {
                _mercadoCache = mercadoCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<MercadoCachedResponse>> Handle(GetMercadoByNomeQuery query, CancellationToken cancellationToken)
            {
                var mercado = await _mercadoCache.GetByNomeAsync(query.Nome);
                var mappedMercado = _mapper.Map<MercadoCachedResponse>(mercado);
                return Result<MercadoCachedResponse>.Success(mappedMercado);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}