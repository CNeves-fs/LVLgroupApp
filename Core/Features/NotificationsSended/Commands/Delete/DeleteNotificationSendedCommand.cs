using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificationsSended.Commands.Delete
{
    public class DeleteNotificationSendedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteNotificationSendedCommandHandler : IRequestHandler<DeleteNotificationSendedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly INotificationSendedRepository _notificationSendedRepository;
            
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteNotificationSendedCommandHandler(INotificationSendedRepository notificationSendedRepository, IUnitOfWork unitOfWork)
            {
                _notificationSendedRepository = notificationSendedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteNotificationSendedCommand command, CancellationToken cancellationToken)
            {
                var notificationSended = await _notificationSendedRepository.GetByIdAsync(command.Id);
                await _notificationSendedRepository.DeleteAsync(notificationSended);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(notificationSended.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}