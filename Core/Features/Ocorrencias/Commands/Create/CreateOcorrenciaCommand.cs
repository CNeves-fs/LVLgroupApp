using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Commands.Create
{
    public partial class CreateOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-OCORR-LLLL-XXXXX

        public DateTime DataOcorrencia { get; set; }

        public string EmailAutor { get; set; }

        public DateTime DataEntradaSistemaOcorrencia { get; set; }





        public int CategoriaId { get; set; }

        public int InterOcorrenciaId { get; set; }          // interlojas

        public bool MasterOcorrencia { get; set; }          // interlojas

        public int TipoOcorrenciaId { get; set; }

        public int StatusId { get; set; }






        public string OcorrenciaNome { get; set; }

        public string Descrição { get; set; }

        public string Comentário { get; set; }

        public int TotalFicheiros { get; set; }

        public string OcorrenciaFolder { get; set; }





        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateOcorrenciaCommandHandler : IRequestHandler<CreateOcorrenciaCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaRepository _ocorrenciaRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateOcorrenciaCommandHandler(IOcorrenciaRepository ocorrenciaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ocorrenciaRepository = ocorrenciaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            var ocorrencia = _mapper.Map<Ocorrencia>(request);
            await _ocorrenciaRepository.InsertAsync(ocorrencia);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(ocorrencia.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}