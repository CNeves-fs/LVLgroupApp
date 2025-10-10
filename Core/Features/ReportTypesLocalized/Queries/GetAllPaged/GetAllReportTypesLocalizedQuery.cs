using AspNetCoreHero.Results;
using Core.Entities.Reports;
using Core.Extensions;
using Core.Features.ReportTypesLocalized.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypesLocalized.Queries.GetAllPaged
{
    public class GetAllReportTypesLocalizedQuery : IRequest<PaginatedResult<ReportTypeLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTypesLocalizedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportTypesLocalizedQueryHandler : IRequestHandler<GetAllReportTypesLocalizedQuery, PaginatedResult<ReportTypeLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTypeLocalizedRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTypesLocalizedQueryHandler(IReportTypeLocalizedRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ReportTypeLocalizedCachedResponse>> Handle(GetAllReportTypesLocalizedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ReportTypeLocalized, ReportTypeLocalizedCachedResponse>> expression = tol => new ReportTypeLocalizedCachedResponse
            {
                Id = tol.Id,
                ReportTypeId = tol.ReportTypeId,
                Language = tol.Language,
                Name = tol.Name
            };
            var paginatedList = await _repository.ReportTypesLocalized
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}