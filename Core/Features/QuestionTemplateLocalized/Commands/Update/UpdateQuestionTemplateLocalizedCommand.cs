using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplateLocalized.Commands.Update
{
    public class UpdateQuestionTemplateLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string QuestionText { get; set; }

        public string Language { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateQuestionTemplateLocalizedCommandHandler : IRequestHandler<UpdateQuestionTemplateLocalizedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IQuestionTemplateLocalizedRepository _questionTemplateLocalizedRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateQuestionTemplateLocalizedCommandHandler(IQuestionTemplateLocalizedRepository questionTemplateLocalizedRepository, IUnitOfWork unitOfWork)
            {
                _questionTemplateLocalizedRepository = questionTemplateLocalizedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateQuestionTemplateLocalizedCommand command, CancellationToken cancellationToken)
            {
                var questionTemplateLocalized = await _questionTemplateLocalizedRepository.GetByIdAsync(command.Id);

                if (questionTemplateLocalized == null)
                {
                    return Result<int>.Fail($"QuestionTemplateLocalized Not Found.");
                }
                else
                {
                    questionTemplateLocalized.QuestionTemplateId = command.QuestionTemplateId == 0 ? questionTemplateLocalized.QuestionTemplateId : command.QuestionTemplateId;
                    questionTemplateLocalized.Language = string.IsNullOrEmpty(command.Language) ? questionTemplateLocalized.Language : command.Language;
                    questionTemplateLocalized.QuestionText = string.IsNullOrEmpty(command.QuestionText) ? questionTemplateLocalized.QuestionText : command.QuestionText;
                    
                    await _questionTemplateLocalizedRepository.UpdateAsync(questionTemplateLocalized);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(questionTemplateLocalized.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}