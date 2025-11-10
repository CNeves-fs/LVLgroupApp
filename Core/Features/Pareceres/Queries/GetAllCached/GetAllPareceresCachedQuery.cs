using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Pareceres.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Pareceres.Queries.GetAllCached
{
    public class GetAllPareceresCachedQuery : IRequest<Result<List<ParecerCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllPareceresCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPareceresCachedQueryHandler : IRequestHandler<GetAllPareceresCachedQuery, Result<List<ParecerCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IParecerCacheRepository _parecerCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllPareceresCachedQueryHandler(IParecerCacheRepository parecerCache, IMapper mapper)
        {
            _parecerCache = parecerCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ParecerCachedResponse>>> Handle(GetAllPareceresCachedQuery request, CancellationToken cancellationToken)
        {
            var parecerList = await _parecerCache.GetCachedListAsync();
            var mappedPareceres = _mapper.Map<List<ParecerCachedResponse>>(parecerList);
            return Result<List<ParecerCachedResponse>>.Success(mappedPareceres);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}