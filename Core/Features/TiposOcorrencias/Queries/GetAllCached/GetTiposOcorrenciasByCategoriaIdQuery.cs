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
    public class GetTiposOcorrenciasByCategoriaIdQuery : IRequest<Result<List<TipoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int CategoriaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetTiposOcorrenciasByCategoriaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetTiposOcorrenciasByCategoriaIdQueryHandler : IRequestHandler<GetTiposOcorrenciasByCategoriaIdQuery, Result<List<TipoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaCacheRepository _tipoOcorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetTiposOcorrenciasByCategoriaIdQueryHandler(ITipoOcorrenciaCacheRepository tipoOcorrenciaCache, IMapper mapper)
        {
            _tipoOcorrenciaCache = tipoOcorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<TipoOcorrenciaCachedResponse>>> Handle(GetTiposOcorrenciasByCategoriaIdQuery query, CancellationToken cancellationToken)
        {
            var tipoOcorrenciasList = await _tipoOcorrenciaCache.GetByCategoriaIdCachedListAsync(query.CategoriaId);
            var mappedTiposOcorrencias = _mapper.Map<List<TipoOcorrenciaCachedResponse>>(tipoOcorrenciasList);
            return Result<List<TipoOcorrenciaCachedResponse>>.Success(mappedTiposOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}