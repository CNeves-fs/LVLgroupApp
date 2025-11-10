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
    public class GetAllGruposlojasByEmpresaIdCachedQuery : IRequest<Result<List<GrupolojasCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllGruposlojasByEmpresaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllGruposlojasByEmpresaIdCachedQueryHandler : IRequestHandler<GetAllGruposlojasByEmpresaIdCachedQuery, Result<List<GrupolojasCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGrupolojaCacheRepository _grupolojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllGruposlojasByEmpresaIdCachedQueryHandler(IGrupolojaCacheRepository grupolojaCache, IMapper mapper)
        {
            _grupolojaCache = grupolojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<GrupolojasCachedResponse>>> Handle(GetAllGruposlojasByEmpresaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var grupolojaList = await _grupolojaCache.GetByEmpresaIdCachedListAsync(request.empresaId);
            var mappedGruposgrupolojas = _mapper.Map<List<GrupolojasCachedResponse>>(grupolojaList);
            return Result<List<GrupolojasCachedResponse>>.Success(mappedGruposgrupolojas);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}