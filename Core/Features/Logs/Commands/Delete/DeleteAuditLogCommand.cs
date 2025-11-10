using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Logs.Commands.Delete
{
    public class DeleteAuditLogCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteAuditLogCommandHandler : IRequestHandler<DeleteAuditLogCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ILogRepository _logRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteAuditLogCommandHandler(ILogRepository logRepository, IUnitOfWork unitOfWork)
            {
                _logRepository = logRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteAuditLogCommand command, CancellationToken cancellationToken)
            {
                var log = await _logRepository.GetByIdAsync(command.Id);
                await _logRepository.DeleteAsync(log);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(log.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}