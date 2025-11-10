using AspNetCoreHero.Results;
using Core.Entities.Notifications;
using Core.Extensions;
using Core.Features.NotificationsSended.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificationsSended.Queries.GetAllPaged
{
    public class GetAllPagedNotificationsSendedQuery : IRequest<PaginatedResult<NotificationSendedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }

        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedNotificationsSendedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedNotificationsSendedQueryHandler : IRequestHandler<GetAllPagedNotificationsSendedQuery, PaginatedResult<NotificationSendedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationSendedRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedNotificationsSendedQueryHandler(INotificationSendedRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<NotificationSendedCachedResponse>> Handle(GetAllPagedNotificationsSendedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<NotificationSended, NotificationSendedCachedResponse>> expression = nau => new NotificationSendedCachedResponse
            {
                Id = nau.Id,
                NotificationId = nau.NotificationId,
                ToUserId = nau.ToUserId,
                IsRead = nau.IsRead
            };
            var paginatedList = await _repository.NotificationsSended
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}