using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fotos.Commands.Delete
{
    public class DeleteFotoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteFotoCommandHandler : IRequestHandler<DeleteFotoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IFotoRepository _fotoRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteFotoCommandHandler(IFotoRepository fotoRepository, IUnitOfWork unitOfWork)
            {
                _fotoRepository = fotoRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteFotoCommand command, CancellationToken cancellationToken)
            {
                var foto = await _fotoRepository.GetByIdAsync(command.Id);
                await _fotoRepository.DeleteAsync(foto);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(foto.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}