using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Statuss.Commands.Delete
{
    public class DeleteStatusCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteStatusCommandHandler : IRequestHandler<DeleteStatusCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IStatusRepository _statusRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteStatusCommandHandler(IStatusRepository statusRepository, IUnitOfWork unitOfWork)
            {
                _statusRepository = statusRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteStatusCommand command, CancellationToken cancellationToken)
            {
                var status = await _statusRepository.GetByIdAsync(command.Id);
                await _statusRepository.DeleteAsync(status);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(status.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}