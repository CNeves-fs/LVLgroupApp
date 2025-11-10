using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Extensions;
using Core.Features.QuestionTemplate.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Queries.GetAllPaged
{
    public class GetAllPagedQuestionTemplateQuery : IRequest<PaginatedResult<QuestionTemplateCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedQuestionTemplateQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedQuestionTemplateQueryHandler : IRequestHandler<GetAllPagedQuestionTemplateQuery, PaginatedResult<QuestionTemplateCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedQuestionTemplateQueryHandler(IQuestionTemplateRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<QuestionTemplateCachedResponse>> Handle(GetAllPagedQuestionTemplateQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Reports.QuestionTemplate, QuestionTemplateCachedResponse>> expression = q => new QuestionTemplateCachedResponse
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                QuestionTypeId = q.QuestionTypeId,
                Version = q.Version,
                IsActive = q.IsActive,
                CreatedAt = q.CreatedAt
            };
            var paginatedList = await _repository.QuestionTemplates
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}