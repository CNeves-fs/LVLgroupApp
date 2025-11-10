using AspNetCoreHero.Results;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Genders.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Queries.GetAllPaged
{
    public class GetAllGendersQuery : IRequest<PaginatedResult<GenderCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllGendersQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllGendersQueryHandler : IRequestHandler<GetAllGendersQuery, PaginatedResult<GenderCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGenderRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllGendersQueryHandler(IGenderRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<GenderCachedResponse>> Handle(GetAllGendersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Gender, GenderCachedResponse>> expression = g => new GenderCachedResponse
            {
                Id = g.Id,
                Nome = g.Nome,
                TamanhosAlf = g.TamanhosAlf,
                TamanhosNum = g.TamanhosNum
            };
            var paginatedList = await _repository.Genders
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}