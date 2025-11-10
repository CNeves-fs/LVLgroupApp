using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Statuss.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Statuss.Queries.GetById
{
    public class GetStatusByIdQuery : IRequest<Result<StatusCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetStatusByIdQueryHandler : IRequestHandler<GetStatusByIdQuery, Result<StatusCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IStatusCacheRepository _statusCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetStatusByIdQueryHandler(IStatusCacheRepository statusCache, IMapper mapper)
            {
                _statusCache = statusCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<StatusCachedResponse>> Handle(GetStatusByIdQuery query, CancellationToken cancellationToken)
            {
                var status = await _statusCache.GetByIdAsync(query.Id);
                var mappedStatus = _mapper.Map<StatusCachedResponse>(status);
                return Result<StatusCachedResponse>.Success(mappedStatus);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}