using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Genders.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Queries.GetByNome
{
    public class GetGenderByNomeQuery : IRequest<Result<GenderCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Nome { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetGenderByNomeQueryHandler : IRequestHandler<GetGenderByNomeQuery, Result<GenderCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IGenderCacheRepository _genderCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetGenderByNomeQueryHandler(IGenderCacheRepository genderCache, IMapper mapper)
            {
                _genderCache = genderCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<GenderCachedResponse>> Handle(GetGenderByNomeQuery query, CancellationToken cancellationToken)
            {
                var gender = await _genderCache.GetByNomeAsync(query.Nome);
                var mappedGender = _mapper.Map<GenderCachedResponse>(gender);
                return Result<GenderCachedResponse>.Success(mappedGender);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}