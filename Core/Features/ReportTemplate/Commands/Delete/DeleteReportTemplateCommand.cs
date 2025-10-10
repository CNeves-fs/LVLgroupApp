using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Commands.Delete
{
    public class DeleteReportTemplateCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteReportTemplateCommandHandler : IRequestHandler<DeleteReportTemplateCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTemplateRepository _reportTemplateRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteReportTemplateCommandHandler(IReportTemplateRepository reportTemplateRepository, IUnitOfWork unitOfWork)
            {
                _reportTemplateRepository = reportTemplateRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteReportTemplateCommand command, CancellationToken cancellationToken)
            {
                var reportTemplate = await _reportTemplateRepository.GetByIdAsync(command.Id);
                await _reportTemplateRepository.DeleteAsync(reportTemplate);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(reportTemplate.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}