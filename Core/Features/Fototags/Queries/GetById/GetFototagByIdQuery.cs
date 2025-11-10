using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Fototags.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fototags.Queries.GetById
{
    public class GetFototagByIdQuery : IRequest<Result<FototagCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetFototagByIdQueryHandler : IRequestHandler<GetFototagByIdQuery, Result<FototagCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IFototagCacheRepository _fototagCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetFototagByIdQueryHandler(IFototagCacheRepository fototagCache, IMapper mapper)
            {
                _fototagCache = fototagCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<FototagCachedResponse>> Handle(GetFototagByIdQuery query, CancellationToken cancellationToken)
            {
                var grupoloja = await _fototagCache.GetByIdAsync(query.Id);
                var mappedFototag = _mapper.Map<FototagCachedResponse>(grupoloja);
                return Result<FototagCachedResponse>.Success(mappedFototag);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}