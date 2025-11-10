using AspNetCoreHero.Results;
using AutoMapper;
using Core.Enums;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Report.Commands.Create
{
    public partial class CreateReportCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        public string EmailAutor { get; set; }

        public DateTime ReportDate { get; set; }

        public bool IncluirWeather { get; set; }

        public Weather Weather { get; set; }

        public string Language { get; set; }

        public string Observacoes { get; set; }




        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateReportCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var report = _mapper.Map<Core.Entities.Reports.Report>(request);
            await _reportRepository.InsertAsync(report);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(report.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}