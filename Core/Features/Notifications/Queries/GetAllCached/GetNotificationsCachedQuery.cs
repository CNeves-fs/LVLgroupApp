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
    public class GetNotificationsCachedQuery : IRequest<Result<List<NotificationCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetNotificationsCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotificationsCachedQueryHandler : IRequestHandler<GetNotificationsCachedQuery, Result<List<NotificationCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationCacheRepository _notificationCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsCachedQueryHandler(INotificationCacheRepository notificationCache, IMapper mapper)
        {
            _notificationCache = notificationCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificationCachedResponse>>> Handle(GetNotificationsCachedQuery request, CancellationToken cancellationToken)
        {
            var notificationList = await _notificationCache.GetCachedListAsync();
            var mappedNotifications = _mapper.Map<List<NotificationCachedResponse>>(notificationList);
            return Result<List<NotificationCachedResponse>>.Success(mappedNotifications);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}