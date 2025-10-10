using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Commands.Update
{
    public class UpdateClienteCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string NIF { get; set; }

        public string Telefone { get; set; }

        public string IBAN { get; set; }

        public string Morada { get; set; }

        public DateTime? DataUltimoContacto { get; set; }

        public string TipoContacto { get; set; }

        public string DescriçãoContacto { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IClienteRepository _clienteRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
            {
                _clienteRepository = clienteRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateClienteCommand command, CancellationToken cancellationToken)
            {
                var cliente = await _clienteRepository.GetByIdAsync(command.Id);

                if (cliente == null)
                {
                    return Result<int>.Fail($"Cliente Not Found.");
                }
                else
                {
                    cliente.Nome = command.Nome ?? cliente.Nome;
                    cliente.Email = command.Email ?? cliente.Email;
                    cliente.Telefone = command.Telefone ?? cliente.Telefone;
                    cliente.NIF = command.NIF ?? cliente.NIF;
                    cliente.IBAN = command.IBAN ?? cliente.IBAN;
                    cliente.Morada = command.Morada ?? cliente.Morada;
                    cliente.DataUltimoContacto = command.DataUltimoContacto ?? cliente.DataUltimoContacto;
                    cliente.TipoContacto = command.TipoContacto ?? cliente.TipoContacto;
                    cliente.DescriçãoContacto = command.DescriçãoContacto ?? cliente.DescriçãoContacto;
                    await _clienteRepository.UpdateAsync(cliente);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(cliente.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}