using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Clientes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Queries.GetByTelefone
{
    public class GetClienteByTelefoneQuery : IRequest<Result<ClienteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Telefone { get; set; }


        public class GetClienteByTelefoneQueryHandler : IRequestHandler<GetClienteByTelefoneQuery, Result<ClienteCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClienteCacheRepository _clienteCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetClienteByTelefoneQueryHandler(IClienteCacheRepository clienteCache, IMapper mapper)
            {
                _clienteCache = clienteCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ClienteCachedResponse>> Handle(GetClienteByTelefoneQuery query, CancellationToken cancellationToken)
            {
                var cliente = await _clienteCache.GetByTelefoneAsync(query.Telefone);
                var mappedCliente = _mapper.Map<ClienteCachedResponse>(cliente);
                return Result<ClienteCachedResponse>.Success(mappedCliente);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}