using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplateQuestion.Commands.Update
{
    public class UpdateReportTemplateQuestionCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        public int QuestionTemplateId { get; set; }

        public int QuestionTypeId { get; set; }

        public int Order { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateReportTemplateQuestionCommandHandler : IRequestHandler<UpdateReportTemplateQuestionCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IReportTemplateQuestionRepository _reportTemplateQuestionRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateReportTemplateQuestionCommandHandler(IReportTemplateQuestionRepository reportTemplateQuestionRepository, IUnitOfWork unitOfWork)
            {
                _reportTemplateQuestionRepository = reportTemplateQuestionRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateReportTemplateQuestionCommand command, CancellationToken cancellationToken)
            {
                var reportTemplateQuestion = await _reportTemplateQuestionRepository.GetByIdAsync(command.Id);

                if (reportTemplateQuestion == null)
                {
                    return Result<int>.Fail($"Report Template Question Not Found.");
                }
                else
                {
                    reportTemplateQuestion.ReportTemplateId = (command.ReportTemplateId == 0) ? reportTemplateQuestion.ReportTemplateId : command.ReportTemplateId;
                    reportTemplateQuestion.QuestionTemplateId = (command.QuestionTemplateId == 0) ? reportTemplateQuestion.QuestionTemplateId : command.QuestionTemplateId;
                    reportTemplateQuestion.QuestionTypeId = (command.QuestionTypeId == 0) ? reportTemplateQuestion.QuestionTemplateId : command.QuestionTypeId;
                    reportTemplateQuestion.Order = (command.Order == 0) ? reportTemplateQuestion.Order : command.Order;

                    await _reportTemplateQuestionRepository.UpdateAsync(reportTemplateQuestion);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(reportTemplateQuestion.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}