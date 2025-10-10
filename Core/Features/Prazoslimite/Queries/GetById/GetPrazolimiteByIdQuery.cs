using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Prazoslimite.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Prazoslimite.Queries.GetById
{
    public class GetPrazolimiteByIdQuery : IRequest<Result<PrazolimiteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetPrazolimiteByIdQueryHandler : IRequestHandler<GetPrazolimiteByIdQuery, Result<PrazolimiteCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IPrazolimiteCacheRepository _prazolimiteCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetPrazolimiteByIdQueryHandler(IPrazolimiteCacheRepository prazolimiteCache, IMapper mapper)
            {
                _prazolimiteCache = prazolimiteCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<PrazolimiteCachedResponse>> Handle(GetPrazolimiteByIdQuery query, CancellationToken cancellationToken)
            {
                var prazolimite = await _prazolimiteCache.GetByIdAsync(query.Id);
                var mappedPrazolimite = _mapper.Map<PrazolimiteCachedResponse>(prazolimite);
                return Result<PrazolimiteCachedResponse>.Success(mappedPrazolimite);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}