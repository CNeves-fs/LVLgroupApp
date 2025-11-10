using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Lojas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Queries.GetAllCached
{
    public class GetLojasByMercadoIdCachedQuery : IRequest<Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int mercadoId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetLojasByMercadoIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetLojasByMercadoIdCachedQueryHandler : IRequestHandler<GetLojasByMercadoIdCachedQuery, Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaCacheRepository _lojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetLojasByMercadoIdCachedQueryHandler(ILojaCacheRepository lojaCache, IMapper mapper)
        {
            _lojaCache = lojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<LojaCachedResponse>>> Handle(GetLojasByMercadoIdCachedQuery request, CancellationToken cancellationToken)
        {
            var lojaList = await _lojaCache.GetByMercadoIdCachedListAsync(request.mercadoId);
            var mappedLojas = _mapper.Map<List<LojaCachedResponse>>(lojaList);
            return Result<List<LojaCachedResponse>>.Success(mappedLojas);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}