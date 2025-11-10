using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Statuss.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Statuss.Queries.GetAllCached
{
    public class GetAllStatusByTypeCachedQuery : IRequest<Result<List<StatusCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int statustype { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllStatusByTypeCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllStatusByTypeCachedQueryHandler : IRequestHandler<GetAllStatusByTypeCachedQuery, Result<List<StatusCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStatusCacheRepository _statusCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllStatusByTypeCachedQueryHandler(IStatusCacheRepository statusCache, IMapper mapper)
        {
            _statusCache = statusCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<StatusCachedResponse>>> Handle(GetAllStatusByTypeCachedQuery request, CancellationToken cancellationToken)
        {
            var statusList = await _statusCache.GetCachedTipoListAsync(request.statustype);
            var mappedStats = _mapper.Map<List<StatusCachedResponse>>(statusList);
            return Result<List<StatusCachedResponse>>.Success(mappedStats);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}