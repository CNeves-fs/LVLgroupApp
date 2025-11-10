using AspNetCoreHero.Results;
using AutoMapper;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplateQuestion.Commands.Create
{
    public partial class CreateReportTemplateQuestionCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        public int QuestionTemplateId { get; set; }

        public int QuestionTypeId { get; set; }

        public int Order { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateReportTemplateQuestionCommandHandler : IRequestHandler<CreateReportTemplateQuestionCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateQuestionRepository _reportTemplateQuestionRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateReportTemplateQuestionCommandHandler(IReportTemplateQuestionRepository reportTemplateQuestionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reportTemplateQuestionRepository = reportTemplateQuestionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateReportTemplateQuestionCommand request, CancellationToken cancellationToken)
        {
            var reportTemplateQuestion = _mapper.Map<Core.Entities.Reports.ReportTemplateQuestion>(request);
            await _reportTemplateQuestionRepository.InsertAsync(reportTemplateQuestion);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(reportTemplateQuestion.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}