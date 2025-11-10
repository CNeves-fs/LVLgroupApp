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
    public class GetAllQuestionOptionCachedQuery : IRequest<Result<List<QuestionOptionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionOptionCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllQuestionOptionCachedQueryHandler : IRequestHandler<GetAllQuestionOptionCachedQuery, Result<List<QuestionOptionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionOptionCacheRepository _questionOptionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionOptionCachedQueryHandler(IQuestionOptionCacheRepository questionOptionCache, IMapper mapper)
        {
            _questionOptionCache = questionOptionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionOptionCachedResponse>>> Handle(GetAllQuestionOptionCachedQuery request, CancellationToken cancellationToken)
        {
            var questionOptionList = await _questionOptionCache.GetCachedAllQuestionOptionAsync();
            var mappedQuestionOption = _mapper.Map<List<QuestionOptionCachedResponse>>(questionOptionList);
            return Result<List<QuestionOptionCachedResponse>>.Success(mappedQuestionOption);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}