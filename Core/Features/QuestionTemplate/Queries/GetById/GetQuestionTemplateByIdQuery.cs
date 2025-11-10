using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionTemplate.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Queries.GetById
{
    public class GetQuestionTemplateByIdQuery : IRequest<Result<QuestionTemplateCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetQuestionTemplateByIdQueryHandler : IRequestHandler<GetQuestionTemplateByIdQuery, Result<QuestionTemplateCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IQuestionTemplateCacheRepository _questionTemplateCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetQuestionTemplateByIdQueryHandler(IQuestionTemplateCacheRepository questionTemplateCache, IMapper mapper)
            {
                _questionTemplateCache = questionTemplateCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<QuestionTemplateCachedResponse>> Handle(GetQuestionTemplateByIdQuery query, CancellationToken cancellationToken)
            {
                var questionTemplate = await _questionTemplateCache.GetByIdAsync(query.Id);
                var mappedQuestionTemplate = _mapper.Map<QuestionTemplateCachedResponse>(questionTemplate);
                return Result<QuestionTemplateCachedResponse>.Success(mappedQuestionTemplate);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}