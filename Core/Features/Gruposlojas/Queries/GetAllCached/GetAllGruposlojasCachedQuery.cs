using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Gruposlojas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Gruposlojas.Queries.GetAllCached
{
    public class GetAllGruposlojasCachedQuery : IRequest<Result<List<GrupolojasCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllGruposlojasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllGruposlojasCachedQueryHandler : IRequestHandler<GetAllGruposlojasCachedQuery, Result<List<GrupolojasCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGrupolojaCacheRepository _grupolojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllGruposlojasCachedQueryHandler(IGrupolojaCacheRepository grupolojaCache, IMapper mapper)
        {
            _grupolojaCache = grupolojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<GrupolojasCachedResponse>>> Handle(GetAllGruposlojasCachedQuery request, CancellationToken cancellationToken)
        {
            var grupolojaList = await _grupolojaCache.GetCachedListAsync();
            var mappedGruposlojas = _mapper.Map<List<GrupolojasCachedResponse>>(grupolojaList);
            return Result<List<GrupolojasCachedResponse>>.Success(mappedGruposlojas);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}