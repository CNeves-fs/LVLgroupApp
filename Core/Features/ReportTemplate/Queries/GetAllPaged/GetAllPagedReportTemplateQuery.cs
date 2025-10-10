using AspNetCoreHero.Results;
using Core.Extensions;
using Core.Features.ReportTemplate.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Queries.GetAllPaged
{
    public class GetAllPagedReportTemplateQuery : IRequest<PaginatedResult<ReportTemplateCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedReportTemplateQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedReportTemplateQueryHandler : IRequestHandler<GetAllPagedReportTemplateQuery, PaginatedResult<ReportTemplateCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedReportTemplateQueryHandler(IReportTemplateRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ReportTemplateCachedResponse>> Handle(GetAllPagedReportTemplateQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Reports.ReportTemplate, ReportTemplateCachedResponse>> expression = rt => new ReportTemplateCachedResponse
            {
                Id = rt.Id,
                Name = rt.Name,
                ReportTypeId =rt.ReportTypeId,
                Version = rt.Version,
                IsActive = rt.IsActive,
                CreatedAt = rt.CreatedAt
            };
            var paginatedList = await _repository.ReportTemplates
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}