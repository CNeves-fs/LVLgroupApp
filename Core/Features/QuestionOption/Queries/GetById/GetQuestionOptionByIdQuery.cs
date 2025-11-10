using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionOption.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Queries.GetById
{
    public class GetQuestionOptionByIdQuery : IRequest<Result<QuestionOptionCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetQuestionOptionByIdQueryHandler : IRequestHandler<GetQuestionOptionByIdQuery, Result<QuestionOptionCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IQuestionOptionCacheRepository _questionOptionCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetQuestionOptionByIdQueryHandler(IQuestionOptionCacheRepository questionOptionCache, IMapper mapper)
            {
                _questionOptionCache = questionOptionCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<QuestionOptionCachedResponse>> Handle(GetQuestionOptionByIdQuery query, CancellationToken cancellationToken)
            {
                var questionOption = await _questionOptionCache.GetByIdAsync(query.Id);
                var mappedQuestionOption = _mapper.Map<QuestionOptionCachedResponse>(questionOption);
                return Result<QuestionOptionCachedResponse>.Success(mappedQuestionOption);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}