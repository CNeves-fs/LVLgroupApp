using AspNetCoreHero.Results;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Pareceres.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Pareceres.Queries.GetAllPaged
{
    public class GetAllPareceresQuery : IRequest<PaginatedResult<ParecerCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPareceresQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPareceresQueryHandler : IRequestHandler<GetAllPareceresQuery, PaginatedResult<ParecerCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IParecerRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPareceresQueryHandler(IParecerRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ParecerCachedResponse>> Handle(GetAllPareceresQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Parecer, ParecerCachedResponse>> expression = p => new ParecerCachedResponse
            {
                Id = p.Id,
                Email = p.Email,
                Data = p.Data,
                Opinião = p.Opinião,
            };
            var paginatedList = await _repository.Pareceres
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}