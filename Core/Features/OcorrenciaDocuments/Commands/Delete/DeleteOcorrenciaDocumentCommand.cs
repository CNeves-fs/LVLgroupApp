using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.OcorrenciaDocuments.Commands.Delete
{
    public class DeleteOcorrenciaDocumentCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteOcorrenciaDocumentCommandHandler : IRequestHandler<DeleteOcorrenciaDocumentCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IOcorrenciaDocumentRepository _ocorrenciaDocumentRepository;

            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteOcorrenciaDocumentCommandHandler(IOcorrenciaDocumentRepository ocorrenciaDocumentRepository, IUnitOfWork unitOfWork)
            {
                _ocorrenciaDocumentRepository = ocorrenciaDocumentRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteOcorrenciaDocumentCommand command, CancellationToken cancellationToken)
            {
                var ocorrenciaDocument = await _ocorrenciaDocumentRepository.GetByIdAsync(command.Id);
                await _ocorrenciaDocumentRepository.DeleteAsync(ocorrenciaDocument);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(ocorrenciaDocument.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}