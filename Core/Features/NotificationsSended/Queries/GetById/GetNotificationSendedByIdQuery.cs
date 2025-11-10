using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.NotificationsSended.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificationsSended.Queries.GetById
{
    public class GetNotificationSendedByIdQuery : IRequest<Result<NotificationSendedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetNotificationSendedByIdQueryHandler : IRequestHandler<GetNotificationSendedByIdQuery, Result<NotificationSendedCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly INotificationSendedCacheRepository _notificationSendedCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetNotificationSendedByIdQueryHandler(INotificationSendedCacheRepository notificationSendedCache, IMapper mapper)
            {
                _notificationSendedCache = notificationSendedCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<NotificationSendedCachedResponse>> Handle(GetNotificationSendedByIdQuery query, CancellationToken cancellationToken)
            {
                var notificationSended = await _notificationSendedCache.GetByIdAsync(query.Id);
                var mappedNotificationSended = _mapper.Map<NotificationSendedCachedResponse>(notificationSended);
                return Result<NotificationSendedCachedResponse>.Success(mappedNotificationSended);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}