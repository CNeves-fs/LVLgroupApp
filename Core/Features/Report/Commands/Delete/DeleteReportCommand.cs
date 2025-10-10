using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Report.Commands.Delete
{
    public class DeleteReportCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportRepository _reportRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteReportCommandHandler(IReportRepository reportRepository, IUnitOfWork unitOfWork)
            {
                _reportRepository = reportRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteReportCommand command, CancellationToken cancellationToken)
            {
                var report = await _reportRepository.GetByIdAsync(command.Id);
                await _reportRepository.DeleteAsync(report);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(report.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}