using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionOption.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Queries.GetAllActiveCached
{
    public class GetAllActiveQuestionOptionCachedQuery : IRequest<Result<List<QuestionOptionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllActiveQuestionOptionCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllActiveQuestionOptionCachedQueryHandler : IRequestHandler<GetAllActiveQuestionOptionCachedQuery, Result<List<QuestionOptionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionOptionCacheRepository _questionOptionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllActiveQuestionOptionCachedQueryHandler(IQuestionOptionCacheRepository questionOptionCache, IMapper mapper)
        {
            _questionOptionCache = questionOptionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionOptionCachedResponse>>> Handle(GetAllActiveQuestionOptionCachedQuery request, CancellationToken cancellationToken)
        {
            var questionOptionList = await _questionOptionCache.GetCachedAllActiveQuestionOptionAsync();
            var mappedQuestionOption = _mapper.Map<List<QuestionOptionCachedResponse>>(questionOptionList);
            return Result<List<QuestionOptionCachedResponse>>.Success(mappedQuestionOption);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}