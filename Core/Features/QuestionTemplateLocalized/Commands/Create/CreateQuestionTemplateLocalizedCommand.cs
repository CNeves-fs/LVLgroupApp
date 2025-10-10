using AspNetCoreHero.Results;
using AutoMapper;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplateLocalized.Commands.Create
{
    public partial class CreateQuestionTemplateLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionTemplateId { get; set; }

        public string QuestionText { get; set; }

        public string Language { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateQuestionTemplateLocalizedCommandHandler : IRequestHandler<CreateQuestionTemplateLocalizedCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateLocalizedRepository _questionTemplateLocalizedRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateQuestionTemplateLocalizedCommandHandler(IQuestionTemplateLocalizedRepository questionTemplateLocalizedRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _questionTemplateLocalizedRepository = questionTemplateLocalizedRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateQuestionTemplateLocalizedCommand request, CancellationToken cancellationToken)
        {
            var questionTemplateLocalized = _mapper.Map<Core.Entities.Reports.QuestionTemplateLocalized>(request);
            await _questionTemplateLocalizedRepository.InsertAsync(questionTemplateLocalized);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(questionTemplateLocalized.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}