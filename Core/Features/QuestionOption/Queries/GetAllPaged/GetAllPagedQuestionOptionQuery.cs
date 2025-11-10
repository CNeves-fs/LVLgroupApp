using AspNetCoreHero.Results;
using Core.Extensions;
using Core.Features.QuestionOption.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Queries.GetAllPaged
{
    public class GetAllPagedQuestionOptionQuery : IRequest<PaginatedResult<QuestionOptionCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedQuestionOptionQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedQuestionOptionQueryHandler : IRequestHandler<GetAllPagedQuestionOptionQuery, PaginatedResult<QuestionOptionCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionOptionRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedQuestionOptionQueryHandler(IQuestionOptionRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<QuestionOptionCachedResponse>> Handle(GetAllPagedQuestionOptionQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Reports.QuestionOption, QuestionOptionCachedResponse>> expression = q => new QuestionOptionCachedResponse
            {
                Id = q.Id,
                OptionText_pt = q.OptionText_pt,
                OptionText_es = q.OptionText_es,
                OptionText_en = q.OptionText_en,
                QuestionTemplateId = q.QuestionTemplateId,
                Order = q.Order,
                IsActive = q.IsActive
            };
            var paginatedList = await _repository.QuestionOptions
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}