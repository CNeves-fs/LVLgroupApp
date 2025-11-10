using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Commands.Delete
{
    public class DeleteVendaSemanalCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteVendaSemanalCommandHandler : IRequestHandler<DeleteVendaSemanalCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaSemanalRepository _vendaSemanalRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteVendaSemanalCommandHandler(IVendaSemanalRepository vendaSemanalRepository, IUnitOfWork unitOfWork)
            {
                _vendaSemanalRepository = vendaSemanalRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteVendaSemanalCommand command, CancellationToken cancellationToken)
            {
                var vendaSemanal = await _vendaSemanalRepository.GetByIdAsync(command.Id);
                await _vendaSemanalRepository.DeleteAsync(vendaSemanal);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(vendaSemanal.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}