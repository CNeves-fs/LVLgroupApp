using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Commands.Update
{
    public class UpdateVendaSemanalCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime DataInicialDaSemana { get; set; }

        public DateTime DataFinalDaSemana { get; set; }

        public int NumeroDaSemana { get; set; } = 0;

        public int Mes { get; set; }  // relativo à start date

        public int Quarter { get; set; }  // relativo à start date

        public int Ano { get; set; }  // ano a que pertence a semana

        public Double ValorTotalDaVenda { get; set; }

        public Double ValorTotalDaVendaDoAnoAnterior { get; set; }

        public Double ObjetivoDaVendaSemanal { get; set; }

        public Double VariaçaoAnual { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateVendaSemanalCommandHandler : IRequestHandler<UpdateVendaSemanalCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IVendaSemanalRepository _vendaSemanalRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateVendaSemanalCommandHandler(IVendaSemanalRepository vendaSemanalRepository, IUnitOfWork unitOfWork)
            {
                _vendaSemanalRepository = vendaSemanalRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateVendaSemanalCommand command, CancellationToken cancellationToken)
            {
                var vendaSemanal = await _vendaSemanalRepository.GetByIdAsync(command.Id);

                if (vendaSemanal == null)
                {
                    return Result<int>.Fail($"Venda Semanal Not Found.");
                }
                else
                {
                    DateTime dt = DateTime.ParseExact("01/01/2000", "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    vendaSemanal.DataInicialDaSemana = (command.DataInicialDaSemana.CompareTo(dt) < 0) ? vendaSemanal.DataInicialDaSemana : command.DataInicialDaSemana;
                    vendaSemanal.DataFinalDaSemana = (command.DataFinalDaSemana.CompareTo(dt) < 0) ? vendaSemanal.DataFinalDaSemana : command.DataFinalDaSemana;

                    vendaSemanal.Ano = (command.Ano == 0) ? vendaSemanal.Ano : command.Ano;
                    vendaSemanal.Quarter = (command.Quarter == 0) ? vendaSemanal.Quarter : command.Quarter;
                    vendaSemanal.Mes = (command.Mes == 0) ? vendaSemanal.Mes : command.Mes;
                    vendaSemanal.NumeroDaSemana = (command.NumeroDaSemana == 0) ? vendaSemanal.NumeroDaSemana : command.NumeroDaSemana;

                    vendaSemanal.ValorTotalDaVenda = (command.ValorTotalDaVenda == 0) ? vendaSemanal.ValorTotalDaVenda : command.ValorTotalDaVenda;
                    vendaSemanal.ValorTotalDaVendaDoAnoAnterior = (command.ValorTotalDaVendaDoAnoAnterior == 0) ? vendaSemanal.ValorTotalDaVendaDoAnoAnterior : command.ValorTotalDaVendaDoAnoAnterior;
                    vendaSemanal.ObjetivoDaVendaSemanal = (command.ObjetivoDaVendaSemanal == 0) ? vendaSemanal.ObjetivoDaVendaSemanal : command.ObjetivoDaVendaSemanal;
                    vendaSemanal.VariaçaoAnual = (command.VariaçaoAnual == 0) ? vendaSemanal.VariaçaoAnual : command.VariaçaoAnual;

                    vendaSemanal.LojaId = (command.LojaId == 0) ? vendaSemanal.LojaId : command.LojaId;
                    vendaSemanal.GrupolojaId = (command.GrupolojaId == 0) ? vendaSemanal.GrupolojaId : command.GrupolojaId;
                    vendaSemanal.EmpresaId = (command.EmpresaId == 0) ? vendaSemanal.EmpresaId : command.EmpresaId;
                    vendaSemanal.MercadoId = (command.MercadoId == 0) ? vendaSemanal.MercadoId : command.MercadoId;

                    await _vendaSemanalRepository.UpdateAsync(vendaSemanal);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(vendaSemanal.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}