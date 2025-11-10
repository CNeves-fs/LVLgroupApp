using AspNetCoreHero.Results;
using AutoMapper;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Commands.Create
{
    public partial class CreateReportTemplateCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Name { get; set; }

        public int ReportTypeId { get; set; } // Ex: "Inspeção de Stock"

        public int Version { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateReportTemplateCommandHandler : IRequestHandler<CreateReportTemplateCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateRepository _reportTemplateRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateReportTemplateCommandHandler(IReportTemplateRepository reportTemplateRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reportTemplateRepository = reportTemplateRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateReportTemplateCommand request, CancellationToken cancellationToken)
        {
            var reportTemplate = _mapper.Map<Core.Entities.Reports.ReportTemplate>(request);
            await _reportTemplateRepository.InsertAsync(reportTemplate);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(reportTemplate.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}