using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.NotificationsSended.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificationsSended.Queries.GetAllCached
{
    public class GetNotReadNotificationsSendedByToUserIdCachedQuery : IRequest<Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public string userId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetNotReadNotificationsSendedByToUserIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotReadNotificationsSendedByToUserIdCachedQueryHandler : IRequestHandler<GetNotReadNotificationsSendedByToUserIdCachedQuery, Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationSendedCacheRepository _notificationSendedCache;
        
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotReadNotificationsSendedByToUserIdCachedQueryHandler(INotificationSendedCacheRepository notificationSendedCache, IMapper mapper)
        {
            _notificationSendedCache = notificationSendedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificationSendedCachedResponse>>> Handle(GetNotReadNotificationsSendedByToUserIdCachedQuery request, CancellationToken cancellationToken)
        {
            var notificationsSendedList = await _notificationSendedCache.GetByToUserIdNotReadCachedListAsync(request.userId);
            var mappedNotificationsSended = _mapper.Map<List<NotificationSendedCachedResponse>>(notificationsSendedList);
            return Result<List<NotificationSendedCachedResponse>>.Success(mappedNotificationsSended);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}