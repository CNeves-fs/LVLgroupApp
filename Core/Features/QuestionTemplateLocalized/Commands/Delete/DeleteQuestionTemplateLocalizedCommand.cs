using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Commands.Delete
{
    public class DeleteQuestionTemplateLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteQuestionTemplateLocalizedCommandHandler : IRequestHandler<DeleteQuestionTemplateLocalizedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IQuestionTemplateLocalizedRepository _questionTemplateLocalizedRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteQuestionTemplateLocalizedCommandHandler(IQuestionTemplateLocalizedRepository questionTemplateLocalizedRepository, IUnitOfWork unitOfWork)
            {
                _questionTemplateLocalizedRepository = questionTemplateLocalizedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteQuestionTemplateLocalizedCommand command, CancellationToken cancellationToken)
            {
                var questionTemplateLocalized = await _questionTemplateLocalizedRepository.GetByIdAsync(command.Id);
                await _questionTemplateLocalizedRepository.DeleteAsync(questionTemplateLocalized);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(questionTemplateLocalized.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}