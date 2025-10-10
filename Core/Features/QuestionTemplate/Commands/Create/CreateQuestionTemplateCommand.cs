using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Entities.Reports;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.QuestionTemplate.Commands.Create
{
    public partial class CreateQuestionTemplateCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string QuestionText { get; set; }

        public int QuestionTypeId { get; set; }

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateQuestionTemplateCommandHandler : IRequestHandler<CreateQuestionTemplateCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IQuestionTemplateRepository _questionTemplateRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateQuestionTemplateCommandHandler(IQuestionTemplateRepository questionTemplateRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _questionTemplateRepository = questionTemplateRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateQuestionTemplateCommand request, CancellationToken cancellationToken)
        {
            var questionTemplate = _mapper.Map<Core.Entities.Reports.QuestionTemplate>(request);
            await _questionTemplateRepository.InsertAsync(questionTemplate);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(questionTemplate.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}