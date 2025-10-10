using AspNetCoreHero.Results;
using AutoMapper;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionOption.Commands.Create
{
    public partial class CreateQuestionOptionCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int QuestionOptionId { get; set; }

        public string OptionText_pt { get; set; }

        public string OptionText_en { get; set; }

        public string OptionText_es { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateQuestionOptionCommandHandler : IRequestHandler<CreateQuestionOptionCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionOptionRepository _questionOptionRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateQuestionOptionCommandHandler(IQuestionOptionRepository questionOptionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _questionOptionRepository = questionOptionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            var questionOption = _mapper.Map<Core.Entities.Reports.QuestionOption>(request);
            await _questionOptionRepository.InsertAsync(questionOption);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(questionOption.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}