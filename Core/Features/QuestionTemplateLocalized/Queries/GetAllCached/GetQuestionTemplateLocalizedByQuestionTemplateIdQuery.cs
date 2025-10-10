using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.QuestionTemplateLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplateLocalized.Queries.GetAllCached
{
    public class GetQuestionTemplateLocalizedByQuestionTemplateIdQuery : IRequest<Result<List<QuestionTemplateLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int questionTemplateId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetQuestionTemplateLocalizedByQuestionTemplateIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetQuestionTemplateLocalizedByQuestionTemplateIdQueryHandler : IRequestHandler<GetQuestionTemplateLocalizedByQuestionTemplateIdQuery, Result<List<QuestionTemplateLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateLocalizedCacheRepository _questionTemplateLocalizedCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetQuestionTemplateLocalizedByQuestionTemplateIdQueryHandler(IQuestionTemplateLocalizedCacheRepository questionTemplateLocalizedCache, IMapper mapper)
        {
            _questionTemplateLocalizedCache = questionTemplateLocalizedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionTemplateLocalizedCachedResponse>>> Handle(GetQuestionTemplateLocalizedByQuestionTemplateIdQuery query, CancellationToken cancellationToken)
        {
            var tiposOcorrenciasLocalizedList = await _questionTemplateLocalizedCache.GetByQuestionTemplateIdAsync(query.questionTemplateId);
            var mappedQuestionTemplateLocalized = _mapper.Map<List<QuestionTemplateLocalizedCachedResponse>>(tiposOcorrenciasLocalizedList);
            return Result<List<QuestionTemplateLocalizedCachedResponse>>.Success(mappedQuestionTemplateLocalized);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}