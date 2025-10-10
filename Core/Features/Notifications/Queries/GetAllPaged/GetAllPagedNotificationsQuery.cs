using AspNetCoreHero.Results;
using Core.Extensions;
using Core.Entities.Notifications;
using Core.Features.Notifications.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Notifications.Queries.GetAllPaged
{
    public class GetAllPagedNotificationsQuery : IRequest<PaginatedResult<NotificationCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedNotificationsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedNotificationsQueryHandler : IRequestHandler<GetAllPagedNotificationsQuery, PaginatedResult<NotificationCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedNotificationsQueryHandler(INotificationRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<NotificationCachedResponse>> Handle(GetAllPagedNotificationsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Notification, NotificationCachedResponse>> expression = n => new NotificationCachedResponse
            {
                Id = n.Id,
                Date = n.Date,
                FromUserId = n.FromUserId,
                Subject = n.Subject,
                Text = n.Text
            };
            var paginatedList = await _repository.Notifications
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}