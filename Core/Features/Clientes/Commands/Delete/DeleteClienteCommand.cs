using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Commands.Delete
{
    public class DeleteClienteCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClienteRepository _clienteRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
            {
                _clienteRepository = clienteRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteClienteCommand command, CancellationToken cancellationToken)
            {
                var cliente = await _clienteRepository.GetByIdAsync(command.Id);
                await _clienteRepository.DeleteAsync(cliente);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(cliente.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}