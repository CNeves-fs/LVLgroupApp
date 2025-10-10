using AspNetCoreHero.Results;
using Core.Entities.Vendas;
using Core.Extensions;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetAllPaged
{
    public class GetAllPagedVendasDiariasQuery : IRequest<PaginatedResult<VendaDiariaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedVendasDiariasQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedVendasDiariasQueryHandler : IRequestHandler<GetAllPagedVendasDiariasQuery, PaginatedResult<VendaDiariaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaDiariaRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedVendasDiariasQueryHandler(IVendaDiariaRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<VendaDiariaCachedResponse>> Handle(GetAllPagedVendasDiariasQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<VendaDiaria, VendaDiariaCachedResponse>> expression = vd => new VendaDiariaCachedResponse
            {
                Id = vd.Id,
                VendaSemanalId = vd.VendaSemanalId,
                LojaId = vd.LojaId,
                Ano = vd.Ano,
                Mês = vd.Mês,
                DiaDoMês = vd.DiaDoMês,
                DiaDaSemana = vd.DiaDaSemana,
                DataDaVenda = vd.DataDaVenda,
                ValorDaVenda = vd.ValorDaVenda
            };
            var paginatedList = await _repository.VendasDiarias
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}