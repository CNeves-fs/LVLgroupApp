using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Commands.Delete
{
    public class DeleteVendaDiariaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteVendaDiariaCommandHandler : IRequestHandler<DeleteVendaDiariaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaDiariaRepository _vendaDiariaRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteVendaDiariaCommandHandler(IVendaDiariaRepository vendaDiariaRepository, IUnitOfWork unitOfWork)
            {
                _vendaDiariaRepository = vendaDiariaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteVendaDiariaCommand command, CancellationToken cancellationToken)
            {
                var vendaDiaria = await _vendaDiariaRepository.GetByIdAsync(command.Id);
                await _vendaDiariaRepository.DeleteAsync(vendaDiaria);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(vendaDiaria.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}