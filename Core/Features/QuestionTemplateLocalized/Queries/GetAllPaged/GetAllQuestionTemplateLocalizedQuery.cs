using AspNetCoreHero.Results;
using Core.Entities.Reports;
using Core.Extensions;
using Core.Features.QuestionTemplateLocalized.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplateLocalized.Queries.GetAllPaged
{
    public class GetAllQuestionTemplateLocalizedQuery : IRequest<PaginatedResult<QuestionTemplateLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionTemplateLocalizedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllQuestionTemplateLocalizedQueryHandler : IRequestHandler<GetAllQuestionTemplateLocalizedQuery, PaginatedResult<QuestionTemplateLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateLocalizedRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionTemplateLocalizedQueryHandler(IQuestionTemplateLocalizedRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<QuestionTemplateLocalizedCachedResponse>> Handle(GetAllQuestionTemplateLocalizedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Reports.QuestionTemplateLocalized, QuestionTemplateLocalizedCachedResponse>> expression = qtl => new QuestionTemplateLocalizedCachedResponse
            {
                Id = qtl.Id,
                QuestionTemplateId = qtl.QuestionTemplateId,
                Language = qtl.Language,
                QuestionText = qtl.QuestionText
            };
            var paginatedList = await _repository.QuestionTemplatesLocalized
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}