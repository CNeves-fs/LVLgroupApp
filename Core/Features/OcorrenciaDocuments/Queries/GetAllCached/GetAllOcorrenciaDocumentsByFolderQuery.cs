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
    public class GetAllOcorrenciaDocumentsByFolderQuery : IRequest<Result<List<OcorrenciaDocumentCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Folder { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciaDocumentsByFolderQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllOcorrenciaDocumentsByFolderQueryHandler : IRequestHandler<GetAllOcorrenciaDocumentsByFolderQuery, Result<List<OcorrenciaDocumentCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaDocumentCacheRepository _ocorrenciaDocumentCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciaDocumentsByFolderQueryHandler(IOcorrenciaDocumentCacheRepository ocorrenciaDocumentCache, IMapper mapper)
        {
            _ocorrenciaDocumentCache = ocorrenciaDocumentCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaDocumentCachedResponse>>> Handle(GetAllOcorrenciaDocumentsByFolderQuery query, CancellationToken cancellationToken)
        {
            var ocorrenciasDocumentsList = await _ocorrenciaDocumentCache.GetByFolderAsync(query.Folder);
            var mappedOcorrenciasDocuments = _mapper.Map<List<OcorrenciaDocumentCachedResponse>>(ocorrenciasDocumentsList);
            return Result<List<OcorrenciaDocumentCachedResponse>>.Success(mappedOcorrenciasDocuments);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}