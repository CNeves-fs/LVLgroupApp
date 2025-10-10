using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Commands.Delete
{
    public class DeleteQuestionTemplateCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteQuestionTemplateCommandHandler : IRequestHandler<DeleteQuestionTemplateCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IQuestionTemplateRepository _questionTemplateRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteQuestionTemplateCommandHandler(IQuestionTemplateRepository questionTemplateRepository, IUnitOfWork unitOfWork)
            {
                _questionTemplateRepository = questionTemplateRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteQuestionTemplateCommand command, CancellationToken cancellationToken)
            {
                var questionTemplate = await _questionTemplateRepository.GetByIdAsync(command.Id);
                await _questionTemplateRepository.DeleteAsync(questionTemplate);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(questionTemplate.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}