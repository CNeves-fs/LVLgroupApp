using AspNetCoreHero.Results;
using AutoMapper;
using Core.Interfaces.Repositories;
using Core.Entities.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Notifications.Commands.Create
{
    public partial class CreateNotificationCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string FromUserId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = _mapper.Map<Notification>(request);
            await _notificationRepository.InsertAsync(notification);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(notification.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}