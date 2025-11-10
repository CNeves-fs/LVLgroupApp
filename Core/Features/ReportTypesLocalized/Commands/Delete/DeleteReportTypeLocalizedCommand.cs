using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypesLocalized.Commands.Delete
{
    public class DeleteReportTypeLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteReportTypeLocalizedCommandHandler : IRequestHandler<DeleteReportTypeLocalizedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTypeLocalizedRepository _reportTypeLocalizedRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteReportTypeLocalizedCommandHandler(IReportTypeLocalizedRepository reportTypeLocalizedRepository, IUnitOfWork unitOfWork)
            {
                _reportTypeLocalizedRepository = reportTypeLocalizedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteReportTypeLocalizedCommand command, CancellationToken cancellationToken)
            {
                var reportTypeLocalized = await _reportTypeLocalizedRepository.GetByIdAsync(command.Id);
                await _reportTypeLocalizedRepository.DeleteAsync(reportTypeLocalized);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(reportTypeLocalized.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}