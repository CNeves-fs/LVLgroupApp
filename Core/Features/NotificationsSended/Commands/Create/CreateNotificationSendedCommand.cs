using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Notifications;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificationsSended.Commands.Create
{
    public partial class CreateNotificationSendedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int NotificationId { get; set; }

        public string ToUserId { get; set; }

        public bool IsRead { get; set; } = false;


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateNotificationSendedCommandHandler : IRequestHandler<CreateNotificationSendedCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificationSendedRepository _notificationSendedRepository;
        
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateNotificationSendedCommandHandler(INotificationSendedRepository notificationSendedRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _notificationSendedRepository = notificationSendedRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateNotificationSendedCommand request, CancellationToken cancellationToken)
        {
            var notificationSended = _mapper.Map<NotificationSended>(request);
            await _notificationSendedRepository.InsertAsync(notificationSended);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(notificationSended.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}