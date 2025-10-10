using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionTemplateLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Queries.GetById
{
    public class GetQuestionTemplateLocalizedByIdQuery : IRequest<Result<QuestionTemplateLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetQuestionTemplateLocalizedByIdQueryHandler : IRequestHandler<GetQuestionTemplateLocalizedByIdQuery, Result<QuestionTemplateLocalizedCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IQuestionTemplateLocalizedCacheRepository _questionTemplateLocalizedCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetQuestionTemplateLocalizedByIdQueryHandler(IQuestionTemplateLocalizedCacheRepository questionTemplateLocalizedCache, IMapper mapper)
            {
                _questionTemplateLocalizedCache = questionTemplateLocalizedCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<QuestionTemplateLocalizedCachedResponse>> Handle(GetQuestionTemplateLocalizedByIdQuery query, CancellationToken cancellationToken)
            {
                var questionTemplateLocalized = await _questionTemplateLocalizedCache.GetByIdAsync(query.Id);
                var mappedQuestionTemplateLocalized = _mapper.Map<QuestionTemplateLocalizedCachedResponse>(questionTemplateLocalized);
                return Result<QuestionTemplateLocalizedCachedResponse>.Success(mappedQuestionTemplateLocalized);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}