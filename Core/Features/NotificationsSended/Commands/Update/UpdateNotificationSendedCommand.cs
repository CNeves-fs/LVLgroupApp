using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificationsSended.Commands.Update
{
    public class UpdateNotificationSendedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int NotificationId { get; set; }

        public string ToUserId { get; set; }

        public bool IsRead { get; set; } = false;


        //---------------------------------------------------------------------------------------------------


        public class UpdateNotificationSendedCommandHandler : IRequestHandler<UpdateNotificationSendedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly INotificationSendedRepository _notificationSendedRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateNotificationSendedCommandHandler(INotificationSendedRepository notificationSendedRepository, IUnitOfWork unitOfWork)
            {
                _notificationSendedRepository = notificationSendedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateNotificationSendedCommand command, CancellationToken cancellationToken)
            {
                var notificationSended = await _notificationSendedRepository.GetByIdAsync(command.Id);

                if (notificationSended == null)
                {
                    return Result<int>.Fail($"NotificationSended Not Found.");
                }
                else
                {
                    notificationSended.NotificationId = (command.NotificationId == 0) ? notificationSended.NotificationId : command.NotificationId;
                    notificationSended.ToUserId = string.IsNullOrEmpty(command.ToUserId) ? notificationSended.ToUserId : command.ToUserId;
                    notificationSended.IsRead = command.IsRead;

                    await _notificationSendedRepository.UpdateAsync(notificationSended);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(notificationSended.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}