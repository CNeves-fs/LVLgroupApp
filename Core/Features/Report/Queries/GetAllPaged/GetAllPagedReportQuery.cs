using AspNetCoreHero.Results;
using Core.Entities.Reports;
using Core.Extensions;
using Core.Features.Report.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Core.Features.Report.Queries.GetAllPaged
{
    public class GetAllPagedReportQuery : IRequest<PaginatedResult<ReportCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedReportQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedReportQueryHandler : IRequestHandler<GetAllPagedReportQuery, PaginatedResult<ReportCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedReportQueryHandler(IReportRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ReportCachedResponse>> Handle(GetAllPagedReportQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Reports.Report, ReportCachedResponse>> expression = r => new ReportCachedResponse
            {
                Id = r.Id,
                ReportTemplateId = r.ReportTemplateId,
                EmailAutor = r.EmailAutor,
                ReportDate = r.ReportDate,
                IncluirWeather = r.IncluirWeather,
                Weather = r.Weather,
                Language = r.Language,
                Observacoes = r.Observacoes,

                EmpresaId = r.EmpresaId,
                GrupolojaId = r.GrupolojaId,
                LojaId = r.LojaId,
                MercadoId = r.MercadoId
            };
            var paginatedList = await _repository.Reports
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}