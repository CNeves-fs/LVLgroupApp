using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Prazoslimite.Commands.Create
{
    public partial class CreatePrazolimiteCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        public string Alarme { get; set; }

        public int LimiteMin { get; set; }

        public int LimiteMax { get; set; }

        public string Cortexto { get; set; }

        public string Corfundo { get; set; }
    }


    //---------------------------------------------------------------------------------------------------


    public class CreatePrazolimiteCommandHandler : IRequestHandler<CreatePrazolimiteCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IPrazolimiteRepository _prazolimiteRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreatePrazolimiteCommandHandler(IPrazolimiteRepository prazolimiteRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _prazolimiteRepository = prazolimiteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreatePrazolimiteCommand request, CancellationToken cancellationToken)
        {
            var prazolimite = _mapper.Map<Core.Entities.Claims.Prazolimite>(request);
            await _prazolimiteRepository.InsertAsync(prazolimite);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(prazolimite.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}