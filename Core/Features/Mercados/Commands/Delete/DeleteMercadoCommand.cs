using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Mercados.Commands.Delete
{
    public class DeleteMercadoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public class DeleteMercadoCommandHandler : IRequestHandler<DeleteMercadoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IMercadoRepository _mercadoRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteMercadoCommandHandler(IMercadoRepository mercadoRepository, IUnitOfWork unitOfWork)
            {
                _mercadoRepository = mercadoRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteMercadoCommand command, CancellationToken cancellationToken)
            {
                var mercado = await _mercadoRepository.GetByIdAsync(command.Id);
                await _mercadoRepository.DeleteAsync(mercado);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(mercado.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}