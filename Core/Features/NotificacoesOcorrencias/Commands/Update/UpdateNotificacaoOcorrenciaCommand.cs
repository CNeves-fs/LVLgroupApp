using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Enums;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificacoesOcorrencias.Commands.Update
{
    public class UpdateNotificacaoOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public NotificationDestinationType TipoDestino { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserEmail { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateNotificacaoOcorrenciaCommandHandler : IRequestHandler<UpdateNotificacaoOcorrenciaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly INotificacaoOcorrenciaRepository _notificacaoOcorrenciaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateNotificacaoOcorrenciaCommandHandler(INotificacaoOcorrenciaRepository notificacaoOcorrenciaRepository, IUnitOfWork unitOfWork)
            {
                _notificacaoOcorrenciaRepository = notificacaoOcorrenciaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateNotificacaoOcorrenciaCommand command, CancellationToken cancellationToken)
            {
                var notificacaoOcorrencia = await _notificacaoOcorrenciaRepository.GetByIdAsync(command.Id);

                if (notificacaoOcorrencia == null)
                {
                    return Result<int>.Fail($"NotificacaoOcorrencia Not Found.");
                }
                else
                {
                    notificacaoOcorrencia.TipoOcorrenciaId = (command.TipoOcorrenciaId == 0) ? notificacaoOcorrencia.TipoOcorrenciaId : command.TipoOcorrenciaId;
                    notificacaoOcorrencia.TipoDestino = command.TipoDestino;
                    notificacaoOcorrencia.ApplicationUserId = string.IsNullOrEmpty(command.ApplicationUserId) ? notificacaoOcorrencia.ApplicationUserId : command.ApplicationUserId;
                    
                    await _notificacaoOcorrenciaRepository.UpdateAsync(notificacaoOcorrencia);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(notificacaoOcorrencia.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}