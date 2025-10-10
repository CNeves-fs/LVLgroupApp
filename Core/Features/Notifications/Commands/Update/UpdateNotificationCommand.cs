using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Notifications.Commands.Update
{
    public class UpdateNotificationCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string FromUserId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly INotificationRepository _notificationRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateNotificationCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
            {
                _notificationRepository = notificationRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateNotificationCommand command, CancellationToken cancellationToken)
            {
                var notification = await _notificationRepository.GetByIdAsync(command.Id);

                if (notification == null)
                {
                    return Result<int>.Fail($"Notification Not Found.");
                }
                else
                {
                    notification.FromUserId = string.IsNullOrEmpty(command.FromUserId) ? notification.FromUserId : command.FromUserId;
                    notification.Subject = string.IsNullOrEmpty(command.Subject) ? notification.Subject : command.Subject;
                    notification.Text = string.IsNullOrEmpty(command.Text) ? notification.Text : command.Text;
                    notification.Date = command.Date;

                    await _notificationRepository.UpdateAsync(notification);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(notification.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}