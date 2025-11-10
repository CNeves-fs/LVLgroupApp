using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypes.Commands.Update
{
    public class UpdateReportTypeCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string DefaultName { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateReportTypeCommandHandler : IRequestHandler<UpdateReportTypeCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IReportTypeRepository _reportTypeRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateReportTypeCommandHandler(IReportTypeRepository reportTypeRepository, IUnitOfWork unitOfWork)
            {
                _reportTypeRepository = reportTypeRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateReportTypeCommand command, CancellationToken cancellationToken)
            {
                var reportType = await _reportTypeRepository.GetByIdAsync(command.Id);

                if (reportType == null)
                {
                    return Result<int>.Fail($"ReportType Not Found.");
                }
                else
                {
                    reportType.DefaultName = string.IsNullOrEmpty(command.DefaultName) ? reportType.DefaultName : command.DefaultName;

                    await _reportTypeRepository.UpdateAsync(reportType);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(reportType.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}