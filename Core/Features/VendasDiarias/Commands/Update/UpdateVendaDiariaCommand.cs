using AspNetCoreHero.Results;
using Core.Enums;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Commands.Update
{
    public class UpdateVendaDiariaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int VendaSemanalId { get; set; }

        public int LojaId { get; set; }

        public int GrupolojaId { get; set; }

        public int EmpresaId { get; set; }

        public int MercadoId { get; set; }

        public int Ano { get; set; }

        public int Mês { get; set; }

        public int DiaDoMês { get; set; }

        public int DiaDaSemana { get; set; }

        public DateTime DataDaVenda { get; set; }

        public Double ValorDaVenda { get; set; }

        public int TotalArtigos { get; set; }

        public Double PercentConv { get; set; }

        public Weather Weather { get; set; }

        public string Observacoes { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateVendaDiariaCommandHandler : IRequestHandler<UpdateVendaDiariaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IVendaDiariaRepository _vendaDiariaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateVendaDiariaCommandHandler(IVendaDiariaRepository vendaDiariaRepository, IUnitOfWork unitOfWork)
            {
                _vendaDiariaRepository = vendaDiariaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateVendaDiariaCommand command, CancellationToken cancellationToken)
            {
                var vendaDiaria = await _vendaDiariaRepository.GetByIdAsync(command.Id);

                if (vendaDiaria == null)
                {
                    return Result<int>.Fail($"Venda Diária Not Found.");
                }
                else
                {
                    vendaDiaria.VendaSemanalId = (command.VendaSemanalId == 0) ? vendaDiaria.VendaSemanalId : command.VendaSemanalId;
                    vendaDiaria.LojaId = (command.LojaId == 0) ? vendaDiaria.LojaId : command.LojaId;
                    vendaDiaria.GrupolojaId = (command.GrupolojaId == 0) ? vendaDiaria.GrupolojaId : command.GrupolojaId;
                    vendaDiaria.EmpresaId = (command.EmpresaId == 0) ? vendaDiaria.EmpresaId : command.EmpresaId;
                    vendaDiaria.MercadoId = (command.MercadoId == 0) ? vendaDiaria.MercadoId : command.MercadoId;
                    vendaDiaria.Ano = (command.Ano == 0) ? vendaDiaria.Ano : command.Ano;
                    vendaDiaria.Mês = (command.Mês == 0) ? vendaDiaria.Mês : command.Mês;
                    vendaDiaria.DiaDoMês = (command.DiaDoMês == 0) ? vendaDiaria.DiaDoMês : command.DiaDoMês;
                    vendaDiaria.DiaDaSemana = (command.DiaDaSemana == 0) ? vendaDiaria.DiaDaSemana : command.DiaDaSemana;
                    vendaDiaria.DataDaVenda = command.DataDaVenda;
                    vendaDiaria.ValorDaVenda = command.ValorDaVenda;
                    vendaDiaria.TotalArtigos = (command.TotalArtigos == 0) ? vendaDiaria.TotalArtigos : command.TotalArtigos;
                    vendaDiaria.PercentConv = command.PercentConv;
                    vendaDiaria.Weather = (command.Weather == 0) ? vendaDiaria.Weather : command.Weather;
                    vendaDiaria.Observacoes = string.IsNullOrEmpty(command.Observacoes) ? vendaDiaria.Observacoes : command.Observacoes;

                    await _vendaDiariaRepository.UpdateAsync(vendaDiaria);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(vendaDiaria.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}