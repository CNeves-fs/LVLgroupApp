using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Notifications.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Notifications.Queries.GetById
{
    public class GetNotificationByIdQuery : IRequest<Result<NotificationCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, Result<NotificationCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly INotificationCacheRepository _notificationCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetNotificationByIdQueryHandler(INotificationCacheRepository notificationCache, IMapper mapper)
            {
                _notificationCache = notificationCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<NotificationCachedResponse>> Handle(GetNotificationByIdQuery query, CancellationToken cancellationToken)
            {
                var notification = await _notificationCache.GetByIdAsync(query.Id);
                var mappedNotification = _mapper.Map<NotificationCachedResponse>(notification);
                return Result<NotificationCachedResponse>.Success(mappedNotification);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}