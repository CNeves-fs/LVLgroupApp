using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.TiposOcorrenciasLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Queries.GetAllCached
{
    public class GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery : IRequest<Result<List<TipoOcorrenciaLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int TipoOcorrenciaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQueryHandler : IRequestHandler<GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery, Result<List<TipoOcorrenciaLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaLocalizedCacheRepository _tipoOcorrenciaLocalizedCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQueryHandler(ITipoOcorrenciaLocalizedCacheRepository tipoOcorrenciaLocalizedCache, IMapper mapper)
        {
            _tipoOcorrenciaLocalizedCache = tipoOcorrenciaLocalizedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<TipoOcorrenciaLocalizedCachedResponse>>> Handle(GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery query, CancellationToken cancellationToken)
        {
            var tiposOcorrenciasLocalizedList = await _tipoOcorrenciaLocalizedCache.GetByTipoOcorrenciaIdAsync(query.TipoOcorrenciaId);
            var mappedTiposOcorrenciasLocalized = _mapper.Map<List<TipoOcorrenciaLocalizedCachedResponse>>(tiposOcorrenciasLocalizedList);
            return Result<List<TipoOcorrenciaLocalizedCachedResponse>>.Success(mappedTiposOcorrenciasLocalized);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}