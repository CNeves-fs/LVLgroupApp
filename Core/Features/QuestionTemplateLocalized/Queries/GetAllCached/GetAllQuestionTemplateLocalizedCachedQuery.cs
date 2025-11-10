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
    public class GetAllQuestionTemplateLocalizedCachedQuery : IRequest<Result<List<QuestionTemplateLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionTemplateLocalizedCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllQuestionTemplateLocalizedCachedQueryHandler : IRequestHandler<GetAllQuestionTemplateLocalizedCachedQuery, Result<List<QuestionTemplateLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateLocalizedCacheRepository _questionTemplateLocalizedCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllQuestionTemplateLocalizedCachedQueryHandler(IQuestionTemplateLocalizedCacheRepository questionTemplateLocalizedCache, IMapper mapper)
        {
            _questionTemplateLocalizedCache = questionTemplateLocalizedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<QuestionTemplateLocalizedCachedResponse>>> Handle(GetAllQuestionTemplateLocalizedCachedQuery request, CancellationToken cancellationToken)
        {
            var questionTemplateLocalizedList = await _questionTemplateLocalizedCache.GetCachedListAsync();
            var mappedTiposOcorrenciasLocalized = _mapper.Map<List<QuestionTemplateLocalizedCachedResponse>>(questionTemplateLocalizedList);
            return Result<List<QuestionTemplateLocalizedCachedResponse>>.Success(mappedTiposOcorrenciasLocalized);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}