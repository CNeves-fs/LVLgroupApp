using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificacoesOcorrencias.Commands.Delete
{
    public class DeleteNotificacaoOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteNotificacaoOcorrenciaCommandHandler : IRequestHandler<DeleteNotificacaoOcorrenciaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly INotificacaoOcorrenciaRepository _notificacaoOcorrenciaRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteNotificacaoOcorrenciaCommandHandler(INotificacaoOcorrenciaRepository notificacaoOcorrenciaRepository, IUnitOfWork unitOfWork)
            {
                _notificacaoOcorrenciaRepository = notificacaoOcorrenciaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteNotificacaoOcorrenciaCommand command, CancellationToken cancellationToken)
            {
                var notificacaoOcorrencia = await _notificacaoOcorrenciaRepository.GetByIdAsync(command.Id);
                await _notificacaoOcorrenciaRepository.DeleteAsync(notificacaoOcorrencia);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(notificacaoOcorrencia.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}