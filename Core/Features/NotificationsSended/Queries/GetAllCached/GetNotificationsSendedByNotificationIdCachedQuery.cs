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
    public class GetNotificationsSendedByNotificationIdCachedQuery : IRequest<Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int NotificationId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsSendedByNotificationIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotificationsSendedByNotificationIdCachedQueryHandler : IRequestHandler<GetNotificationsSendedByNotificationIdCachedQuery, Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationSendedCacheRepository _notificationSendedCache;
        
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsSendedByNotificationIdCachedQueryHandler(INotificationSendedCacheRepository notificationSendedCache, IMapper mapper)
        {
            _notificationSendedCache = notificationSendedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificationSendedCachedResponse>>> Handle(GetNotificationsSendedByNotificationIdCachedQuery request, CancellationToken cancellationToken)
        {
            var notificationSendedList = await _notificationSendedCache.GetByNotificationIdCachedListAsync(request.NotificationId);
            var mappedNotificationsSended = _mapper.Map<List<NotificationSendedCachedResponse>>(notificationSendedList);
            return Result<List<NotificationSendedCachedResponse>>.Success(mappedNotificationsSended);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}