using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Reports;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypesLocalized.Commands.Create
{
    public partial class CreateReportTypeLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTypeId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateReportTypeLocalizedCommandHandler : IRequestHandler<CreateReportTypeLocalizedCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTypeLocalizedRepository _reportTypeLocalizedRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateReportTypeLocalizedCommandHandler(IReportTypeLocalizedRepository reportTypeLocalizedRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reportTypeLocalizedRepository = reportTypeLocalizedRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateReportTypeLocalizedCommand request, CancellationToken cancellationToken)
        {
            var reportTypeLocalized = _mapper.Map<ReportTypeLocalized>(request);
            await _reportTypeLocalizedRepository.InsertAsync(reportTypeLocalized);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(reportTypeLocalized.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}