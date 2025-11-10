using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.OcorrenciaDocuments.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.OcorrenciaDocuments.Queries.GetById
{
    public class GetOcorrenciaDocumentByIdQuery : IRequest<Result<OcorrenciaDocumentCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetOcorrenciaDocumentByIdQueryHandler : IRequestHandler<GetOcorrenciaDocumentByIdQuery, Result<OcorrenciaDocumentCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IOcorrenciaDocumentCacheRepository _ocorrenciaDocumentCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetOcorrenciaDocumentByIdQueryHandler(IOcorrenciaDocumentCacheRepository ocorrenciaDocumentCache, IMapper mapper)
            {
                _ocorrenciaDocumentCache = ocorrenciaDocumentCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<OcorrenciaDocumentCachedResponse>> Handle(GetOcorrenciaDocumentByIdQuery query, CancellationToken cancellationToken)
            {
                var ocorrenciaDocument = await _ocorrenciaDocumentCache.GetByIdAsync(query.Id);
                var mappedOcorrenciaDocument = _mapper.Map<OcorrenciaDocumentCachedResponse>(ocorrenciaDocument);
                return Result<OcorrenciaDocumentCachedResponse>.Success(mappedOcorrenciaDocument);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}