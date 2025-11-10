using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Prazoslimite.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Prazoslimite.Queries.GetAllPaged
{
    public class GetAllPrazoslimiteQuery : IRequest<PaginatedResult<PrazolimiteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPrazoslimiteQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPrazoslimiteQueryHandler : IRequestHandler<GetAllPrazoslimiteQuery, PaginatedResult<PrazolimiteCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IPrazolimiteRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPrazoslimiteQueryHandler(IPrazolimiteRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<PrazolimiteCachedResponse>> Handle(GetAllPrazoslimiteQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Prazolimite, PrazolimiteCachedResponse>> expression = p => new PrazolimiteCachedResponse
            {
                Id = p.Id,
                Alarme = p.Alarme,
                LimiteMin = p.LimiteMin,
                LimiteMax = p.LimiteMax,
                Cortexto = p.Cortexto,
                Corfundo = p.Corfundo
            };
            var paginatedList = await _repository.Prazoslimite
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}