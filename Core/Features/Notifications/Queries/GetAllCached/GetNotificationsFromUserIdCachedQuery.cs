using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Notifications.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Notifications.Queries.GetAllCached
{
    public class GetNotificationsFromUserIdCachedQuery : IRequest<Result<List<NotificationCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public string userId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsFromUserIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotificationsFromUserIdCachedQueryHandler : IRequestHandler<GetNotificationsFromUserIdCachedQuery, Result<List<NotificationCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationCacheRepository _notificationCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsFromUserIdCachedQueryHandler(INotificationCacheRepository notificationCache, IMapper mapper)
        {
            _notificationCache = notificationCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificationCachedResponse>>> Handle(GetNotificationsFromUserIdCachedQuery request, CancellationToken cancellationToken)
        {
            var notificationList = await _notificationCache.GetByFromUserIdCachedListAsync(request.userId);
            var mappedNotifications = _mapper.Map<List<NotificationCachedResponse>>(notificationList);
            return Result<List<NotificationCachedResponse>>.Success(mappedNotifications);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}