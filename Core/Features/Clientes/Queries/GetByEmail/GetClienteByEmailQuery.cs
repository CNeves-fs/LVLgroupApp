using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Clientes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Queries.GetByEmail
{
    public class GetClienteByEmailQuery : IRequest<Result<ClienteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Email { get; set; }


        public class GetClienteByEmailQueryHandler : IRequestHandler<GetClienteByEmailQuery, Result<ClienteCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClienteCacheRepository _clienteCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetClienteByEmailQueryHandler(IClienteCacheRepository clienteCache, IMapper mapper)
            {
                _clienteCache = clienteCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ClienteCachedResponse>> Handle(GetClienteByEmailQuery query, CancellationToken cancellationToken)
            {
                var cliente = await _clienteCache.GetByEmailAsync(query.Email);
                var mappedCliente = _mapper.Map<ClienteCachedResponse>(cliente);
                return Result<ClienteCachedResponse>.Success(mappedCliente);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}