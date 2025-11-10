using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Reports;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypes.Commands.Create
{
    public partial class CreateReportTypeCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string DefaultName { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateReportTypeCommandHandler : IRequestHandler<CreateReportTypeCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTypeRepository _reportTypeRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateReportTypeCommandHandler(IReportTypeRepository reportTypeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reportTypeRepository = reportTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateReportTypeCommand request, CancellationToken cancellationToken)
        {
            var reportType = _mapper.Map<ReportType>(request);
            await _reportTypeRepository.InsertAsync(reportType);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(reportType.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}