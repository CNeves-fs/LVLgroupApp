using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionTemplate.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Queries.GetAllActiveCached
{
    public class GetAllActiveQuestionTemplatesCachedQuery : IRequest<Result<List<QuestionTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllActiveQuestionTemplatesCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllActiveQuestionTemplatesCachedQueryHandler : IRequestHandler<GetAllActiveQuestionTemplatesCachedQuery, Result<List<QuestionTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateCacheRepository _questionTemplateCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllActiveQuestionTemplatesCachedQueryHandler(IQuestionTemplateCacheRepository questionTemplateCache, IMapper mapper)
        {
            _questionTemplateCache = questionTemplateCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionTemplateCachedResponse>>> Handle(GetAllActiveQuestionTemplatesCachedQuery request, CancellationToken cancellationToken)
        {
            var questionTemplateList = await _questionTemplateCache.GetCachedAllActiveQuestionTemplateAsync();
            var mappedQuestionTemplates = _mapper.Map<List<QuestionTemplateCachedResponse>>(questionTemplateList);
            return Result<List<QuestionTemplateCachedResponse>>.Success(mappedQuestionTemplates);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}