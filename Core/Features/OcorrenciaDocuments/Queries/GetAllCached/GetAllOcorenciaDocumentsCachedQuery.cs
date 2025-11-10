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
    public class GetAllOcorenciaDocumentsCachedQuery : IRequest<Result<List<OcorrenciaDocumentCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllOcorenciaDocumentsCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllOcorenciaDocumentsCachedQueryHandler : IRequestHandler<GetAllOcorenciaDocumentsCachedQuery, Result<List<OcorrenciaDocumentCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaDocumentCacheRepository _ocorrenciaDocumentCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorenciaDocumentsCachedQueryHandler(IOcorrenciaDocumentCacheRepository ocorrenciaDocumentCache, IMapper mapper)
        {
            _ocorrenciaDocumentCache = ocorrenciaDocumentCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaDocumentCachedResponse>>> Handle(GetAllOcorenciaDocumentsCachedQuery request, CancellationToken cancellationToken)
        {
            var ocorrenciaDocumentList = await _ocorrenciaDocumentCache.GetCachedListAsync();
            var mappedOcorrenciasDocuments = _mapper.Map<List<OcorrenciaDocumentCachedResponse>>(ocorrenciaDocumentList);
            return Result<List<OcorrenciaDocumentCachedResponse>>.Success(mappedOcorrenciasDocuments);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}