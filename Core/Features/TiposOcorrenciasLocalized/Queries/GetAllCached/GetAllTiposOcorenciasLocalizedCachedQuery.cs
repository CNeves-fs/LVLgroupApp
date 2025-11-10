using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.TiposOcorrencias.Response;
using Core.Features.TiposOcorrenciasLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Queries.GetAllCached
{
    public class GetAllTiposOcorenciasLocalizedCachedQuery : IRequest<Result<List<TipoOcorrenciaLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorenciasLocalizedCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllTiposOcorenciasLocalizedCachedQueryHandler : IRequestHandler<GetAllTiposOcorenciasLocalizedCachedQuery, Result<List<TipoOcorrenciaLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaLocalizedCacheRepository _tipoOcorrenciaLocalizedCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorenciasLocalizedCachedQueryHandler(ITipoOcorrenciaLocalizedCacheRepository tipoOcorrenciaLocalizedCache, IMapper mapper)
        {
            _tipoOcorrenciaLocalizedCache = tipoOcorrenciaLocalizedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<TipoOcorrenciaLocalizedCachedResponse>>> Handle(GetAllTiposOcorenciasLocalizedCachedQuery request, CancellationToken cancellationToken)
        {
            var tipoOcorrenciaLocalizedList = await _tipoOcorrenciaLocalizedCache.GetCachedListAsync();
            var mappedTiposOcorrenciasLocalized = _mapper.Map<List<TipoOcorrenciaLocalizedCachedResponse>>(tipoOcorrenciaLocalizedList);
            return Result<List<TipoOcorrenciaLocalizedCachedResponse>>.Success(mappedTiposOcorrenciasLocalized);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}