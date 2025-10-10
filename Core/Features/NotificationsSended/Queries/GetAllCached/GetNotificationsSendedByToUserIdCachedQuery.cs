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
    public class GetNotificationsSendedByToUserIdCachedQuery : IRequest<Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public string userId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsSendedByToUserIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotificationsSendedByToUserIdCachedQueryHandler : IRequestHandler<GetNotificationsSendedByToUserIdCachedQuery, Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationSendedCacheRepository _notificationSendedCache;
        
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsSendedByToUserIdCachedQueryHandler(INotificationSendedCacheRepository notificationSendedCache, IMapper mapper)
        {
            _notificationSendedCache = notificationSendedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificationSendedCachedResponse>>> Handle(GetNotificationsSendedByToUserIdCachedQuery request, CancellationToken cancellationToken)
        {
            var notificationSendedList = await _notificationSendedCache.GetByToUserIdCachedListAsync(request.userId);
            var mappedNotificationsSended = _mapper.Map<List<NotificationSendedCachedResponse>>(notificationSendedList);
            return Result<List<NotificationSendedCachedResponse>>.Success(mappedNotificationsSended);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}