using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Vendas;
using Core.Enums;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Commands.Update
{
    public class FastUpdateVendaDiariaCommand : IRequest<Result<int>>
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


        public class FastUpdateVendaDiariaCommandHandler : IRequestHandler<FastUpdateVendaDiariaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IVendaDiariaRepository _vendaDiariaRepository;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public FastUpdateVendaDiariaCommandHandler(IVendaDiariaRepository vendaDiariaRepository, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _vendaDiariaRepository = vendaDiariaRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(FastUpdateVendaDiariaCommand command, CancellationToken cancellationToken)
            {

                var vendaDiaria = _mapper.Map<VendaDiaria>(command);
                await _vendaDiariaRepository.UpdateAsync(vendaDiaria);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(vendaDiaria.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}