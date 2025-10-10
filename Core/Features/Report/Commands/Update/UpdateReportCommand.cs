using AspNetCoreHero.Results;
using Core.Entities.Vendas;
using Core.Enums;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Report.Commands.Update
{
    public class UpdateReportCommand : IRequest<Result<int>>
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


        //---------------------------------------------------------------------------------------------------


        public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IReportRepository _reportRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateReportCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork)
            {
                _reportRepository = reportRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateReportCommand command, CancellationToken cancellationToken)
            {
                var report = await _reportRepository.GetByIdAsync(command.Id);

                if (report == null)
                {
                    return Result<int>.Fail($"Report Not Found.");
                }
                else
                {
                    report.ReportTemplateId = (command.ReportTemplateId == 0) ? report.ReportTemplateId : command.ReportTemplateId;
                    report.EmailAutor = (string.IsNullOrEmpty(command.EmailAutor)) ? report.EmailAutor : command.EmailAutor;
                    report.ReportDate = command.ReportDate;
                    report.IncluirWeather = command.IncluirWeather;
                    report.Weather = (command.Weather == 0) ? report.Weather : command.Weather;
                    report.Language = (string.IsNullOrEmpty(command.Language)) ? report.Language : command.Language;
                    report.Observacoes = string.IsNullOrEmpty(command.Observacoes) ? report.Observacoes : command.Observacoes;

                    report.EmpresaId = (command.EmpresaId == 0) ? report.EmpresaId : command.EmpresaId;
                    report.GrupolojaId = (command.GrupolojaId == 0) ? report.GrupolojaId : command.GrupolojaId;
                    report.LojaId = (command.LojaId == 0) ? report.LojaId : command.LojaId;
                    report.MercadoId = (command.MercadoId == 0) ? report.MercadoId : command.MercadoId;

                    await _reportRepository.UpdateAsync(report);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(report.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}