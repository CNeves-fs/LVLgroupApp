using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Clientes;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Clientes.Commands.Create
{
    public partial class CreateClienteCommand : IRequest<Result<int>>
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

    }

    public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = _mapper.Map<Cliente>(request);
            await _clienteRepository.InsertAsync(cliente);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(cliente.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}