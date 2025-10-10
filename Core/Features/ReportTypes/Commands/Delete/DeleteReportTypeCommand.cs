using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypes.Commands.Delete
{
    public class DeleteReportTypeCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteReportTypeCommandHandler : IRequestHandler<DeleteReportTypeCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTypeRepository _reportTypeRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteReportTypeCommandHandler(IReportTypeRepository reportTypeRepository, IUnitOfWork unitOfWork)
            {
                _reportTypeRepository = reportTypeRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteReportTypeCommand command, CancellationToken cancellationToken)
            {
                var reportType = await _reportTypeRepository.GetByIdAsync(command.Id);
                await _reportTypeRepository.DeleteAsync(reportType);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(reportType.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}