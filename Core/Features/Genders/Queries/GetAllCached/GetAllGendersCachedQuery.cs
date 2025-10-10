using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Genders.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Queries.GetAllCached
{
    public class GetAllGendersCachedQuery : IRequest<Result<List<GenderCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllGendersCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllGendersCachedQueryHandler : IRequestHandler<GetAllGendersCachedQuery, Result<List<GenderCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGenderCacheRepository _genderCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllGendersCachedQueryHandler(IGenderCacheRepository genderCache, IMapper mapper)
        {
            _genderCache = genderCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<GenderCachedResponse>>> Handle(GetAllGendersCachedQuery request, CancellationToken cancellationToken)
        {
            var genderList = await _genderCache.GetCachedListAsync();
            var mappedGenders = _mapper.Map<List<GenderCachedResponse>>(genderList);
            return Result<List<GenderCachedResponse>>.Success(mappedGenders);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}