using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Genders.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Queries.GetById
{
    public class GetGenderByIdQuery : IRequest<Result<GenderCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetGenderByIdQueryHandler : IRequestHandler<GetGenderByIdQuery, Result<GenderCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IGenderCacheRepository _genderCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetGenderByIdQueryHandler(IGenderCacheRepository genderCache, IMapper mapper)
            {
                _genderCache = genderCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<GenderCachedResponse>> Handle(GetGenderByIdQuery query, CancellationToken cancellationToken)
            {
                var grupoloja = await _genderCache.GetByIdAsync(query.Id);
                var mappedGender = _mapper.Map<GenderCachedResponse>(grupoloja);
                return Result<GenderCachedResponse>.Success(mappedGender);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}