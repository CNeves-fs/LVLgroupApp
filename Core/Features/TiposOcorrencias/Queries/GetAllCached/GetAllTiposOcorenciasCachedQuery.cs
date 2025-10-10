using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.TiposOcorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Queries.GetAllCached
{
    public class GetAllTiposOcorenciasCachedQuery : IRequest<Result<List<TipoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorenciasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllTiposOcorenciasCachedQueryHandler : IRequestHandler<GetAllTiposOcorenciasCachedQuery, Result<List<TipoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaCacheRepository _tipoOcorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorenciasCachedQueryHandler(ITipoOcorrenciaCacheRepository tipoOcorrenciaCache, IMapper mapper)
        {
            _tipoOcorrenciaCache = tipoOcorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<TipoOcorrenciaCachedResponse>>> Handle(GetAllTiposOcorenciasCachedQuery request, CancellationToken cancellationToken)
        {
            var tipoOcorrenciaList = await _tipoOcorrenciaCache.GetCachedListAsync();
            var mappedTiposOcorrencias = _mapper.Map<List<TipoOcorrenciaCachedResponse>>(tipoOcorrenciaList);
            return Result<List<TipoOcorrenciaCachedResponse>>.Success(mappedTiposOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}