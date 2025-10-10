using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Commands.Delete
{
    public class DeleteTipoOcorrenciaLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteTipoOcorrenciaLocalizedCommandHandler : IRequestHandler<DeleteTipoOcorrenciaLocalizedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ITipoOcorrenciaLocalizedRepository _tipoOcorrenciaLocalizedRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteTipoOcorrenciaLocalizedCommandHandler(ITipoOcorrenciaLocalizedRepository tipoOcorrenciaLocalizedRepository, IUnitOfWork unitOfWork)
            {
                _tipoOcorrenciaLocalizedRepository = tipoOcorrenciaLocalizedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteTipoOcorrenciaLocalizedCommand command, CancellationToken cancellationToken)
            {
                var tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByIdAsync(command.Id);
                await _tipoOcorrenciaLocalizedRepository.DeleteAsync(tipoOcorrenciaLocalized);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(tipoOcorrenciaLocalized.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}