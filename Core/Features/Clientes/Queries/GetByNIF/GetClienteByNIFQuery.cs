using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Clientes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Queries.GetByNIF
{
    public class GetClienteByNIFQuery : IRequest<Result<ClienteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string NIF { get; set; }


        public class GetClienteByNIFQueryHandler : IRequestHandler<GetClienteByNIFQuery, Result<ClienteCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClienteCacheRepository _clienteCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetClienteByNIFQueryHandler(IClienteCacheRepository clienteCache, IMapper mapper)
            {
                _clienteCache = clienteCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ClienteCachedResponse>> Handle(GetClienteByNIFQuery query, CancellationToken cancellationToken)
            {
                var cliente = await _clienteCache.GetByNIFAsync(query.NIF);
                var mappedCliente = _mapper.Map<ClienteCachedResponse>(cliente);
                return Result<ClienteCachedResponse>.Success(mappedCliente);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}