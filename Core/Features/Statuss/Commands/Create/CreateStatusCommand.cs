using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Statuss.Commands.Create
{
    public partial class CreateStatusCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        public int Tipo { get; set; }

        public string Texto { get; set; }

        public string Cortexto { get; set; }

        public string Corfundo { get; set; }
    }


    //---------------------------------------------------------------------------------------------------


    public class CreateStatusCommandHandler : IRequestHandler<CreateStatusCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateStatusCommandHandler(IStatusRepository statusRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var status = _mapper.Map<Core.Entities.Claims.Status>(request);
            await _statusRepository.InsertAsync(status);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(status.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}