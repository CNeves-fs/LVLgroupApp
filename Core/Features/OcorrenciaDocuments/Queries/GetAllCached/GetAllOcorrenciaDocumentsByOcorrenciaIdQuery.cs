using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.OcorrenciaDocuments.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.OcorrenciaDocuments.Queries.GetAllCached
{
    public class GetAllOcorrenciaDocumentsByOcorrenciaIdQuery : IRequest<Result<List<OcorrenciaDocumentCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int OcorrenciaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciaDocumentsByOcorrenciaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllOcorrenciaDocumentsByOcorrenciaIdQueryHandler : IRequestHandler<GetAllOcorrenciaDocumentsByOcorrenciaIdQuery, Result<List<OcorrenciaDocumentCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaDocumentCacheRepository _ocorrenciaDocumentCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciaDocumentsByOcorrenciaIdQueryHandler(IOcorrenciaDocumentCacheRepository ocorrenciaDocumentCache, IMapper mapper)
        {
            _ocorrenciaDocumentCache = ocorrenciaDocumentCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaDocumentCachedResponse>>> Handle(GetAllOcorrenciaDocumentsByOcorrenciaIdQuery query, CancellationToken cancellationToken)
        {
            var ocorrenciasDocumentsList = await _ocorrenciaDocumentCache.GetByOcorrenciaIdAsync(query.OcorrenciaId);
            var mappedOcorrenciasDocuments = _mapper.Map<List<OcorrenciaDocumentCachedResponse>>(ocorrenciasDocumentsList);
            return Result<List<OcorrenciaDocumentCachedResponse>>.Success(mappedOcorrenciasDocuments);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}