using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplateQuestion.Commands.Delete
{
    public class DeleteReportTemplateQuestionCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteReportTemplateQuestionCommandHandler : IRequestHandler<DeleteReportTemplateQuestionCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTemplateQuestionRepository _reportTemplateQuestionRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteReportTemplateQuestionCommandHandler(IReportTemplateQuestionRepository reportTemplateQuestionRepository, IUnitOfWork unitOfWork)
            {
                _reportTemplateQuestionRepository = reportTemplateQuestionRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteReportTemplateQuestionCommand command, CancellationToken cancellationToken)
            {
                var reportTemplateQuestion = await _reportTemplateQuestionRepository.GetByIdAsync(command.Id);
                await _reportTemplateQuestionRepository.DeleteAsync(reportTemplateQuestion);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(reportTemplateQuestion.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}