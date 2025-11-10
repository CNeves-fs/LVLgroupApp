using AspNetCoreHero.Results;
using Core.Entities.Reports;
using Core.Extensions;
using Core.Features.ReportTemplateQuestion.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplateQuestion.Queries.GetAllPaged
{
    public class GetAllPagedReportTemplateQuestionQuery : IRequest<PaginatedResult<ReportTemplateQuestionCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedReportTemplateQuestionQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedReportTemplateQuestionQueryHandler : IRequestHandler<GetAllPagedReportTemplateQuestionQuery, PaginatedResult<ReportTemplateQuestionCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateQuestionRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedReportTemplateQuestionQueryHandler(IReportTemplateQuestionRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ReportTemplateQuestionCachedResponse>> Handle(GetAllPagedReportTemplateQuestionQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Reports.ReportTemplateQuestion, ReportTemplateQuestionCachedResponse>> expression = rtq => new ReportTemplateQuestionCachedResponse
            {
                Id = rtq.Id,
                ReportTemplateId = rtq.ReportTemplateId,
                QuestionTemplateId = rtq.QuestionTemplateId,
                Order = rtq.Order
            };
            var paginatedList = await _repository.ReportTemplateQuestions
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}