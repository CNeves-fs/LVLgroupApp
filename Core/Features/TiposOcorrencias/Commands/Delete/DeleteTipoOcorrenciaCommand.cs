using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Commands.Delete
{
    public class DeleteTipoOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteTipoOcorrenciaCommandHandler : IRequestHandler<DeleteTipoOcorrenciaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ITipoOcorrenciaRepository _tipoOcorrenciaRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteTipoOcorrenciaCommandHandler(ITipoOcorrenciaRepository tipoOcorrenciaRepository, IUnitOfWork unitOfWork)
            {
                _tipoOcorrenciaRepository = tipoOcorrenciaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteTipoOcorrenciaCommand command, CancellationToken cancellationToken)
            {
                var tipoOcorrencia = await _tipoOcorrenciaRepository.GetByIdAsync(command.Id);
                await _tipoOcorrenciaRepository.DeleteAsync(tipoOcorrencia);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(tipoOcorrencia.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}