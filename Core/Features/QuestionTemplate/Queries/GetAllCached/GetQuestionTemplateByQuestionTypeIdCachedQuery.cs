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
    public class GetQuestionTemplateByQuestionTypeIdCachedQuery : IRequest<Result<List<QuestionTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int questionTypeId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetQuestionTemplateByQuestionTypeIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetQuestionTemplateByQuestionTypeIdCachedQueryHandler : IRequestHandler<GetQuestionTemplateByQuestionTypeIdCachedQuery, Result<List<QuestionTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateCacheRepository _questionTemplateCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetQuestionTemplateByQuestionTypeIdCachedQueryHandler(IQuestionTemplateCacheRepository questionTemplateCache, IMapper mapper)
        {
            _questionTemplateCache = questionTemplateCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionTemplateCachedResponse>>> Handle(GetQuestionTemplateByQuestionTypeIdCachedQuery request, CancellationToken cancellationToken)
        {
            var questionTemplateList = await _questionTemplateCache.GetByTypeIdCachedListAsync(request.questionTypeId);
            var mappedQuestionTemplate = _mapper.Map<List<QuestionTemplateCachedResponse>>(questionTemplateList);
            return Result<List<QuestionTemplateCachedResponse>>.Success(mappedQuestionTemplate);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}