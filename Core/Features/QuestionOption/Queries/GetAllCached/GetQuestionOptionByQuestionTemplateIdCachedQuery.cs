using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionOption.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Queries.GetAllCached
{
    public class GetQuestionOptionByQuestionTemplateIdCachedQuery : IRequest<Result<List<QuestionOptionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int questionTemplateId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetQuestionOptionByQuestionTemplateIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetQuestionOptionByQuestionTemplateIdCachedQueryHandler : IRequestHandler<GetQuestionOptionByQuestionTemplateIdCachedQuery, Result<List<QuestionOptionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionOptionCacheRepository _questionOptionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetQuestionOptionByQuestionTemplateIdCachedQueryHandler(IQuestionOptionCacheRepository questionOptionCache, IMapper mapper)
        {
            _questionOptionCache = questionOptionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionOptionCachedResponse>>> Handle(GetQuestionOptionByQuestionTemplateIdCachedQuery request, CancellationToken cancellationToken)
        {
            var questionOptionList = await _questionOptionCache.GetByQuestionTemplateIdCachedListAsync(request.questionTemplateId);
            var mappedQuestionOption = _mapper.Map<List<QuestionOptionCachedResponse>>(questionOptionList);
            return Result<List<QuestionOptionCachedResponse>>.Success(mappedQuestionOption);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}