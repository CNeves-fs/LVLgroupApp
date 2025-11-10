using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Commands.Update
{
    public class UpdateReportTemplateCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Name { get; set; }

        public int ReportTypeId { get; set; } // Ex: "Inspeção de Stock"

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateReportTemplateCommandHandler : IRequestHandler<UpdateReportTemplateCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IReportTemplateRepository _reportTemplateRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateReportTemplateCommandHandler(IReportTemplateRepository reportTemplateRepository, IUnitOfWork unitOfWork)
            {
                _reportTemplateRepository = reportTemplateRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateReportTemplateCommand command, CancellationToken cancellationToken)
            {
                var reportTemplate = await _reportTemplateRepository.GetByIdAsync(command.Id);

                if (reportTemplate == null)
                {
                    return Result<int>.Fail($"Report Template Not Found.");
                }
                else
                {
                    reportTemplate.Name = string.IsNullOrEmpty(command.Name) ? reportTemplate.Name : command.Name;
                    reportTemplate.ReportTypeId = (command.ReportTypeId == 0) ? reportTemplate.ReportTypeId : command.ReportTypeId;
                    reportTemplate.Version = (command.Version == 0) ? reportTemplate.Version : command.Version;
                    reportTemplate.IsActive = command.IsActive;
                    reportTemplate.CreatedAt = command.CreatedAt;

                    await _reportTemplateRepository.UpdateAsync(reportTemplate);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(reportTemplate.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}