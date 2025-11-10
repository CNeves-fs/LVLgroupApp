using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Enums;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificacoesOcorrencias.Commands.Create
{
    public partial class CreateNotificacaoOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public NotificationDestinationType TipoDestino { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserEmail { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateNotificacaoOcorrenciaCommandHandler : IRequestHandler<CreateNotificacaoOcorrenciaCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificacaoOcorrenciaRepository _notificacaoOcorrenciaRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateNotificacaoOcorrenciaCommandHandler(INotificacaoOcorrenciaRepository notificacaoOcorrenciaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _notificacaoOcorrenciaRepository = notificacaoOcorrenciaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateNotificacaoOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            var notificacaoOcorrencia = _mapper.Map<NotificacaoOcorrencia>(request);
            await _notificacaoOcorrenciaRepository.InsertAsync(notificacaoOcorrencia);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(notificacaoOcorrencia.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}