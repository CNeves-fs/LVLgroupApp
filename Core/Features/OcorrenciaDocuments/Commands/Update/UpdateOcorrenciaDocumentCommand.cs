using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.OcorrenciaDocuments.Commands.Update
{
    public class UpdateOcorrenciaDocumentCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }                // Path no server /wwwroot/Ocorrencias/id/filename

        public int? OcorrenciaId { get; set; }

        public DateTime UploadDate { get; set; }

        public string OcorrenciaFolder { get; set; }

        public string Descrição { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateOcorrenciaDocumentCommandHandler : IRequestHandler<UpdateOcorrenciaDocumentCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IOcorrenciaDocumentRepository _ocorrenciaDocumentRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateOcorrenciaDocumentCommandHandler(IOcorrenciaDocumentRepository ocorrenciaDocumentRepository, IUnitOfWork unitOfWork)
            {
                _ocorrenciaDocumentRepository = ocorrenciaDocumentRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateOcorrenciaDocumentCommand command, CancellationToken cancellationToken)
            {
                var ocorrenciaDocument = await _ocorrenciaDocumentRepository.GetByIdAsync(command.Id);

                if (ocorrenciaDocument == null)
                {
                    return Result<int>.Fail($"OcorrenciaDocument Not Found.");
                }
                else
                {
                    ocorrenciaDocument.FileName = string.IsNullOrEmpty(command.FileName) ? ocorrenciaDocument.FileName : command.FileName;
                    ocorrenciaDocument.FilePath = string.IsNullOrEmpty(command.FilePath) ? ocorrenciaDocument.FilePath : command.FilePath;
                    ocorrenciaDocument.OcorrenciaFolder = string.IsNullOrEmpty(command.OcorrenciaFolder) ? ocorrenciaDocument.OcorrenciaFolder : command.OcorrenciaFolder;
                    
                    ocorrenciaDocument.OcorrenciaId = (command.OcorrenciaId == 0) ? ocorrenciaDocument.OcorrenciaId : command.OcorrenciaId;
                    DateTime dt = DateTime.ParseExact("01/01/2000", "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    ocorrenciaDocument.UploadDate = (command.UploadDate.CompareTo(dt) < 0) ? ocorrenciaDocument.UploadDate : command.UploadDate;
                    ocorrenciaDocument.Descrição = string.IsNullOrEmpty(command.Descrição) ? ocorrenciaDocument.Descrição : command.Descrição;

                    await _ocorrenciaDocumentRepository.UpdateAsync(ocorrenciaDocument);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(ocorrenciaDocument.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}