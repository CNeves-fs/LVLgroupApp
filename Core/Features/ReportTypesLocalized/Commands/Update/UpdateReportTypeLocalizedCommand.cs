using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypesLocalized.Commands.Update
{
    public class UpdateReportTypeLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTypeId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateReportTypeLocalizedCommandHandler : IRequestHandler<UpdateReportTypeLocalizedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IReportTypeLocalizedRepository _reportTypeLocalizedRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateReportTypeLocalizedCommandHandler(IReportTypeLocalizedRepository reportTypeLocalizedRepository, IUnitOfWork unitOfWork)
            {
                _reportTypeLocalizedRepository = reportTypeLocalizedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateReportTypeLocalizedCommand command, CancellationToken cancellationToken)
            {
                var reportTypeLocalized = await _reportTypeLocalizedRepository.GetByIdAsync(command.Id);

                if (reportTypeLocalized == null)
                {
                    return Result<int>.Fail($"ReportTypeLocalized Not Found.");
                }
                else
                {
                    reportTypeLocalized.ReportTypeId = command.ReportTypeId == 0 ? reportTypeLocalized.ReportTypeId : command.ReportTypeId;
                    reportTypeLocalized.Language = string.IsNullOrEmpty(command.Language) ? reportTypeLocalized.Language : command.Language;
                    reportTypeLocalized.Name = string.IsNullOrEmpty(command.Name) ? reportTypeLocalized.Name : command.Name;
                    
                    await _reportTypeLocalizedRepository.UpdateAsync(reportTypeLocalized);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(reportTypeLocalized.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}