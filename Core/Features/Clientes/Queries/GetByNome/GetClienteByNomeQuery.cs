using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Clientes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Queries.GetByNome
{
    public class GetClienteByNomeQuery : IRequest<Result<ClienteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Nome { get; set; }


        public class GetClienteByNomeQueryHandler : IRequestHandler<GetClienteByNomeQuery, Result<ClienteCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClienteCacheRepository _clienteCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetClienteByNomeQueryHandler(IClienteCacheRepository clienteCache, IMapper mapper)
            {
                _clienteCache = clienteCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ClienteCachedResponse>> Handle(GetClienteByNomeQuery query, CancellationToken cancellationToken)
            {
                var cliente = await _clienteCache.GetByNomeAsync(query.Nome);
                var mappedCliente = _mapper.Map<ClienteCachedResponse>(cliente);
                return Result<ClienteCachedResponse>.Success(mappedCliente);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}