using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Vendas;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Commands.Create
{
    public partial class CreateVendaSemanalCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime DataInicialDaSemana { get; set; }

        public DateTime DataFinalDaSemana { get; set; }

        public int NumeroDaSemana { get; set; }

        public int Mes { get; set; }  // relativo à start date

        public int Quarter { get; set; }  // relativo à start date

        public int Ano { get; set; }  // ano a que pertence a semana

        public Double ValorTotalDaVenda { get; set; } = 0;

        public Double ValorTotalDaVendaDoAnoAnterior { get; set; }

        public Double ObjetivoDaVendaSemanal { get; set; }

        public Double VariaçaoAnual { get; set; }
        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateVendaSemanalCommandHandler : IRequestHandler<CreateVendaSemanalCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalRepository _vendaSemanalRepository;

        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateVendaSemanalCommandHandler(IVendaSemanalRepository vendaSemanalRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _vendaSemanalRepository = vendaSemanalRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateVendaSemanalCommand request, CancellationToken cancellationToken)
        {
            var vendaSemanal = _mapper.Map<VendaSemanal>(request);
            await _vendaSemanalRepository.InsertAsync(vendaSemanal);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(vendaSemanal.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}