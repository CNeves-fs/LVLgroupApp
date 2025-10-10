using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Clientes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Queries.GetById
{
    public class GetClienteByIdQuery : IRequest<Result<ClienteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public class GetClienteByIdQueryHandler : IRequestHandler<GetClienteByIdQuery, Result<ClienteCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClienteCacheRepository _clienteCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetClienteByIdQueryHandler(IClienteCacheRepository clienteCache, IMapper mapper)
            {
                _clienteCache = clienteCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ClienteCachedResponse>> Handle(GetClienteByIdQuery query, CancellationToken cancellationToken)
            {
                var cliente = await _clienteCache.GetByIdAsync(query.Id);
                var mappedCliente = _mapper.Map<ClienteCachedResponse>(cliente);
                return Result<ClienteCachedResponse>.Success(mappedCliente);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}