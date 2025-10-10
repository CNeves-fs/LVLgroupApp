using AspNetCoreHero.Results;
using Core.Entities.Vendas;
using Core.Extensions;
using Core.Features.VendasSemanais.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Queries.GetAllPaged
{
    public class GetAllPagedVendasSemanaisQuery : IRequest<PaginatedResult<VendaSemanalCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedVendasSemanaisQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedVendasSemanaisQueryHandler : IRequestHandler<GetAllPagedVendasSemanaisQuery, PaginatedResult<VendaSemanalCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedVendasSemanaisQueryHandler(IVendaSemanalRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<VendaSemanalCachedResponse>> Handle(GetAllPagedVendasSemanaisQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<VendaSemanal, VendaSemanalCachedResponse>> expression = vs => new VendaSemanalCachedResponse
            {
                Id = vs.Id,
                DataInicialDaSemana = vs.DataInicialDaSemana,
                DataFinalDaSemana = vs.DataFinalDaSemana,
                NumeroDaSemana = vs.NumeroDaSemana,
                Mes = vs.Mes,
                Quarter = vs.Quarter,
                Ano = vs.Ano,
                ValorTotalDaVenda = vs.ValorTotalDaVenda,
                ValorTotalDaVendaDoAnoAnterior = vs.ValorTotalDaVendaDoAnoAnterior,
                ObjetivoDaVendaSemanal = vs.ObjetivoDaVendaSemanal,
                VariaçaoAnual = vs.VariaçaoAnual,
                EmpresaId= vs.EmpresaId,
                GrupolojaId = vs.GrupolojaId,
                LojaId = vs.LojaId,
                MercadoId = vs.MercadoId
            };
            var paginatedList = await _repository.VendasSemanais
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}