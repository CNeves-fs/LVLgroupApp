using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Ocorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Queries.GetAllCached
{
    public class GetOcorrenciasByGrupolojaIdQuery : IRequest<Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int GrupolojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByGrupolojaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetOcorrenciasByGrupolojaIdQueryHandler : IRequestHandler<GetOcorrenciasByGrupolojaIdQuery, Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaCacheRepository _ocorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByGrupolojaIdQueryHandler(IOcorrenciaCacheRepository ocorrenciaCache, IMapper mapper)
        {
            _ocorrenciaCache = ocorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaCachedResponse>>> Handle(GetOcorrenciasByGrupolojaIdQuery query, CancellationToken cancellationToken)
        {
            var ocorrenciasList = await _ocorrenciaCache.GetByGrupolojaIdCachedListAsync(query.GrupolojaId);
            var mappedOcorrencias = _mapper.Map<List<OcorrenciaCachedResponse>>(ocorrenciasList);
            return Result<List<OcorrenciaCachedResponse>>.Success(mappedOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}