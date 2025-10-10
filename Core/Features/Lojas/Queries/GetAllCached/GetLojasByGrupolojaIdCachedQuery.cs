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
    public class GetLojasByGrupolojaIdCachedQuery : IRequest<Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int grupolojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetLojasByGrupolojaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetLojasByGrupolojaIdCachedQueryHandler : IRequestHandler<GetLojasByGrupolojaIdCachedQuery, Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaCacheRepository _lojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetLojasByGrupolojaIdCachedQueryHandler(ILojaCacheRepository lojaCache, IMapper mapper)
        {
            _lojaCache = lojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<LojaCachedResponse>>> Handle(GetLojasByGrupolojaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var lojaList = await _lojaCache.GetByGrupolojaIdCachedListAsync(request.grupolojaId);
            var mappedLojas = _mapper.Map<List<LojaCachedResponse>>(lojaList);
            return Result<List<LojaCachedResponse>>.Success(mappedLojas);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}