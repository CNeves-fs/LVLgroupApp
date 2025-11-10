using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Reports;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Commands.Update
{
    public class UpdateQuestionTemplateCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string QuestionText { get; set; }

        public int QuestionTypeId { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateQuestionTemplateCommandHandler : IRequestHandler<UpdateQuestionTemplateCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IQuestionTemplateRepository _questionTemplateRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateQuestionTemplateCommandHandler(IQuestionTemplateRepository questionTemplateRepository, IUnitOfWork unitOfWork)
            {
                _questionTemplateRepository = questionTemplateRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateQuestionTemplateCommand command, CancellationToken cancellationToken)
            {
                var questionTemplate = await _questionTemplateRepository.GetByIdAsync(command.Id);

                if (questionTemplate == null)
                {
                    return Result<int>.Fail($"Question Template Not Found.");
                }
                else
                {
                    questionTemplate.QuestionText = command.QuestionText ?? questionTemplate.QuestionText;
                    questionTemplate.QuestionTypeId = (command.QuestionTypeId == 0) ? questionTemplate.QuestionTypeId : command.QuestionTypeId;
                    questionTemplate.Version = (command.Version == 0) ? questionTemplate.Version : command.Version;
                    questionTemplate.CreatedAt = command.CreatedAt;
                    questionTemplate.IsActive = command.IsActive;

                    await _questionTemplateRepository.UpdateAsync(questionTemplate);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(questionTemplate.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}