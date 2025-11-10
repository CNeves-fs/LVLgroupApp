using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Pareceres.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Pareceres.Queries.GetById
{
    public class GetParecerByIdQuery : IRequest<Result<ParecerCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetParecerByIdQueryHandler : IRequestHandler<GetParecerByIdQuery, Result<ParecerCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IParecerCacheRepository _parecerCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetParecerByIdQueryHandler(IParecerCacheRepository parecerCache, IMapper mapper)
            {
                _parecerCache = parecerCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ParecerCachedResponse>> Handle(GetParecerByIdQuery query, CancellationToken cancellationToken)
            {
                var parecer = await _parecerCache.GetByIdAsync(query.Id);
                var mappedParecer = _mapper.Map<ParecerCachedResponse>(parecer);
                return Result<ParecerCachedResponse>.Success(mappedParecer);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}