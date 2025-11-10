using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Claims.Commands.Delete
{
    public class DeleteClaimCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClaimRepository _claimRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteClaimCommandHandler(IClaimRepository claimRepository, IUnitOfWork unitOfWork)
            {
                _claimRepository = claimRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteClaimCommand command, CancellationToken cancellationToken)
            {
                var claim = await _claimRepository.GetByIdAsync(command.Id);
                await _claimRepository.DeleteAsync(claim);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(claim.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}