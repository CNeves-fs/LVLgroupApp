using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Commands.Create
{
    public partial class CreateTipoOcorrenciaLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateTipoOcorrenciaLocalizedCommandHandler : IRequestHandler<CreateTipoOcorrenciaLocalizedCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaLocalizedRepository _tipoOcorrenciaLocalizedRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateTipoOcorrenciaLocalizedCommandHandler(ITipoOcorrenciaLocalizedRepository tipoOcorrenciaLocalizedRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tipoOcorrenciaLocalizedRepository = tipoOcorrenciaLocalizedRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateTipoOcorrenciaLocalizedCommand request, CancellationToken cancellationToken)
        {
            var tipoOcorrenciaLocalized = _mapper.Map<TipoOcorrenciaLocalized>(request);
            await _tipoOcorrenciaLocalizedRepository.InsertAsync(tipoOcorrenciaLocalized);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(tipoOcorrenciaLocalized.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}