using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Notifications.Commands.Delete
{
    public class DeleteNotificationCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly INotificationRepository _notificationRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteNotificationCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
            {
                _notificationRepository = notificationRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteNotificationCommand command, CancellationToken cancellationToken)
            {
                var notification = await _notificationRepository.GetByIdAsync(command.Id);
                await _notificationRepository.DeleteAsync(notification);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(notification.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}