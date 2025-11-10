using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTemplate.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Queries.GetAllCached
{
    public class GetAllActiveReportTemplateCachedQuery : IRequest<Result<List<ReportTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllActiveReportTemplateCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllActiveReportTemplateCachedQueryHandler : IRequestHandler<GetAllActiveReportTemplateCachedQuery, Result<List<ReportTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateCacheRepository _questionOptionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllActiveReportTemplateCachedQueryHandler(IReportTemplateCacheRepository questionOptionCache, IMapper mapper)
        {
            _questionOptionCache = questionOptionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTemplateCachedResponse>>> Handle(GetAllActiveReportTemplateCachedQuery request, CancellationToken cancellationToken)
        {
            var questionOptionList = await _questionOptionCache.GetCachedAllActiveReportTemplateAsync();
            var mappedReportTemplate = _mapper.Map<List<ReportTemplateCachedResponse>>(questionOptionList);
            return Result<List<ReportTemplateCachedResponse>>.Success(mappedReportTemplate);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}