using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Commands.Delete
{
    public class DeleteQuestionOptionCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteQuestionOptionCommandHandler : IRequestHandler<DeleteQuestionOptionCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IQuestionOptionRepository _questionOptionRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteQuestionOptionCommandHandler(IQuestionOptionRepository questionOptionRepository, IUnitOfWork unitOfWork)
            {
                _questionOptionRepository = questionOptionRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteQuestionOptionCommand command, CancellationToken cancellationToken)
            {
                var questionOption = await _questionOptionRepository.GetByIdAsync(command.Id);
                await _questionOptionRepository.DeleteAsync(questionOption);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(questionOption.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}