using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Commands.Update
{
    public class UpdateQuestionOptionCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string OptionText_pt { get; set; }

        public string OptionText_en { get; set; }

        public string OptionText_es { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateQuestionOptionCommandHandler : IRequestHandler<UpdateQuestionOptionCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IQuestionOptionRepository _questionOptionRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateQuestionOptionCommandHandler(IQuestionOptionRepository questionOptionRepository, IUnitOfWork unitOfWork)
            {
                _questionOptionRepository = questionOptionRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateQuestionOptionCommand command, CancellationToken cancellationToken)
            {
                var questionOption = await _questionOptionRepository.GetByIdAsync(command.Id);

                if (questionOption == null)
                {
                    return Result<int>.Fail($"Question Option Not Found.");
                }
                else
                {
                    questionOption.OptionText_pt = command.OptionText_pt ?? questionOption.OptionText_pt;
                    questionOption.OptionText_es = command.OptionText_es ?? questionOption.OptionText_es;
                    questionOption.OptionText_en = command.OptionText_en ?? questionOption.OptionText_en;
                    questionOption.QuestionTemplateId = (command.QuestionTemplateId == 0) ? questionOption.QuestionTemplateId : command.QuestionTemplateId;
                    questionOption.Order = (command.Order == 0) ? questionOption.Order : command.Order;
                    questionOption.IsActive = command.IsActive;

                    await _questionOptionRepository.UpdateAsync(questionOption);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(questionOption.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}