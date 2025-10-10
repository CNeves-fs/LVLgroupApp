using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Clientes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Queries.GetAllCached
{
    public class GetAllClientesCachedQuery : IRequest<Result<List<ClienteCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllClientesCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }

    public class GetAllClientesCachedQueryHandler : IRequestHandler<GetAllClientesCachedQuery, Result<List<ClienteCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClienteCacheRepository _clienteCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllClientesCachedQueryHandler(IClienteCacheRepository clienteCache, IMapper mapper)
        {
            _clienteCache = clienteCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ClienteCachedResponse>>> Handle(GetAllClientesCachedQuery request, CancellationToken cancellationToken)
        {
            var clienteList = await _clienteCache.GetCachedListAsync();
            var mappedClientes = _mapper.Map<List<ClienteCachedResponse>>(clienteList);
            return Result<List<ClienteCachedResponse>>.Success(mappedClientes);
        }


        //---------------------------------------------------------------------------------------------------

    }
}