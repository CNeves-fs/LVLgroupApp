using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Extensions;
using Core.Features.Lojas.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Queries.GetAllPaged
{
    public class GetAllPagedLojasQuery : IRequest<PaginatedResult<LojaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedLojasQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedLojasQueryHandler : IRequestHandler<GetAllPagedLojasQuery, PaginatedResult<LojaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedLojasQueryHandler(ILojaRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<LojaCachedResponse>> Handle(GetAllPagedLojasQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Loja, LojaCachedResponse>> expression = l => new LojaCachedResponse
            {
                Id = l.Id,
                Nome = l.Nome,
                NomeCurto = l.NomeCurto,
                Cidade = l.Cidade,
                País = l.País,
                GrupolojaId = l.GrupolojaId,
                //GerenteId = l.GerenteId,
                //BasicLojaId = l.BasicLojaId
            };
            var paginatedList = await _repository.Lojas
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}