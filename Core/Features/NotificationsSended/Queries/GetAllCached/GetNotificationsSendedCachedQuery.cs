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
    public class GetNotificationsSendedCachedQuery : IRequest<Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetNotificationsSendedCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotificationsSendedCachedQueryHandler : IRequestHandler<GetNotificationsSendedCachedQuery, Result<List<NotificationSendedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationSendedCacheRepository _notificationSendedCache;
        
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotificationsSendedCachedQueryHandler(INotificationSendedCacheRepository notificationSendedCache, IMapper mapper)
        {
            _notificationSendedCache = notificationSendedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificationSendedCachedResponse>>> Handle(GetNotificationsSendedCachedQuery request, CancellationToken cancellationToken)
        {
            var notificationSendedList = await _notificationSendedCache.GetCachedListAsync();
            var mappedNotificationsSended = _mapper.Map<List<NotificationSendedCachedResponse>>(notificationSendedList);
            return Result<List<NotificationSendedCachedResponse>>.Success(mappedNotificationsSended);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}