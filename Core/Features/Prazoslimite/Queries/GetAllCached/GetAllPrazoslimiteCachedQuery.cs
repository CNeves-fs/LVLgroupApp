using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Prazoslimite.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Prazoslimite.Queries.GetAllCached
{
    public class GetAllPrazoslimiteCachedQuery : IRequest<Result<List<PrazolimiteCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllPrazoslimiteCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPrazoslimiteCachedQueryHandler : IRequestHandler<GetAllPrazoslimiteCachedQuery, Result<List<PrazolimiteCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IPrazolimiteCacheRepository _statusCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllPrazoslimiteCachedQueryHandler(IPrazolimiteCacheRepository statusCache, IMapper mapper)
        {
            _statusCache = statusCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<PrazolimiteCachedResponse>>> Handle(GetAllPrazoslimiteCachedQuery request, CancellationToken cancellationToken)
        {
            var statusList = await _statusCache.GetCachedListAsync();
            var mappedPrazoslimite = _mapper.Map<List<PrazolimiteCachedResponse>>(statusList);
            return Result<List<PrazolimiteCachedResponse>>.Success(mappedPrazoslimite);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}