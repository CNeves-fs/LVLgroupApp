using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionTemplate.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Queries.GetAllCached
{
    public class GetAllQuestionTemplateCachedQuery : IRequest<Result<List<QuestionTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionTemplateCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllQuestionTemplateCachedQueryHandler : IRequestHandler<GetAllQuestionTemplateCachedQuery, Result<List<QuestionTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateCacheRepository _questionTemplateCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionTemplateCachedQueryHandler(IQuestionTemplateCacheRepository questionTemplateCache, IMapper mapper)
        {
            _questionTemplateCache = questionTemplateCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionTemplateCachedResponse>>> Handle(GetAllQuestionTemplateCachedQuery request, CancellationToken cancellationToken)
        {
            var questionTemplateList = await _questionTemplateCache.GetCachedAllQuestionTemplateAsync();
            var mappedQuestionTemplate = _mapper.Map<List<QuestionTemplateCachedResponse>>(questionTemplateList);
            return Result<List<QuestionTemplateCachedResponse>>.Success(mappedQuestionTemplate);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}