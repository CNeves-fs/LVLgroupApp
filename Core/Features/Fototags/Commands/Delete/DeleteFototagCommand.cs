using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fototags.Commands.Delete
{
    public class DeleteFototagCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteFototagCommandHandler : IRequestHandler<DeleteFototagCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IFototagRepository _fototagRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteFototagCommandHandler(IFototagRepository fototagRepository, IUnitOfWork unitOfWork)
            {
                _fototagRepository = fototagRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteFototagCommand command, CancellationToken cancellationToken)
            {
                var fototag = await _fototagRepository.GetByIdAsync(command.Id);
                await _fototagRepository.DeleteAsync(fototag);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(fototag.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}