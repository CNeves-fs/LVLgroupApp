using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.OcorrenciaDocuments.Commands.Create
{
    public partial class CreateOcorrenciaDocumentCommand : IRequest<Result<int>>
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

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateOcorrenciaDocumentCommandHandler : IRequestHandler<CreateOcorrenciaDocumentCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaDocumentRepository _ocorrenciaDocumentRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateOcorrenciaDocumentCommandHandler(IOcorrenciaDocumentRepository ocorrenciaDocumentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ocorrenciaDocumentRepository = ocorrenciaDocumentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateOcorrenciaDocumentCommand request, CancellationToken cancellationToken)
        {
            var ocorrenciaDocument = _mapper.Map<OcorrenciaDocument>(request);
            await _ocorrenciaDocumentRepository.InsertAsync(ocorrenciaDocument);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(ocorrenciaDocument.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}