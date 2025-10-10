using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Commands.Delete
{
    public class DeleteOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteOcorrenciaCommandHandler : IRequestHandler<DeleteOcorrenciaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IOcorrenciaRepository _ocorrenciaRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteOcorrenciaCommandHandler(IOcorrenciaRepository ocorrenciaRepository, IUnitOfWork unitOfWork)
            {
                _ocorrenciaRepository = ocorrenciaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteOcorrenciaCommand command, CancellationToken cancellationToken)
            {
                var ocorrencia = await _ocorrenciaRepository.GetByIdAsync(command.Id);
                await _ocorrenciaRepository.DeleteAsync(ocorrencia);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(ocorrencia.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}