using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Extensions;
using Core.Features.OcorrenciaDocuments.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.OcorrenciaDocuments.Queries.GetAllPaged
{
    public class GetAllOcorrenciaDocumentsQuery : IRequest<PaginatedResult<OcorrenciaDocumentCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciaDocumentsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllOcorrenciaDocumentsQueryHandler : IRequestHandler<GetAllOcorrenciaDocumentsQuery, PaginatedResult<OcorrenciaDocumentCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaDocumentRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciaDocumentsQueryHandler(IOcorrenciaDocumentRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<OcorrenciaDocumentCachedResponse>> Handle(GetAllOcorrenciaDocumentsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<OcorrenciaDocument, OcorrenciaDocumentCachedResponse>> expression = od => new OcorrenciaDocumentCachedResponse
            {
                Id = od.Id,
                FileName = od.FileName,
                FilePath = od.FilePath,
                OcorrenciaId = od.OcorrenciaId,
                UploadDate = od.UploadDate,
                Descrição = od.Descrição
            };
            var paginatedList = await _repository.OcorrenciaDocuments
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}