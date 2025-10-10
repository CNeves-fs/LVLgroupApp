using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Commands.Create
{
    public partial class CreateTipoOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string DefaultName { get; set; }

        public int CategoriaId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateTipoOcorrenciaCommandHandler : IRequestHandler<CreateTipoOcorrenciaCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaRepository _tipoOcorrenciaRepository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateTipoOcorrenciaCommandHandler(ITipoOcorrenciaRepository tipoOcorrenciaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _tipoOcorrenciaRepository = tipoOcorrenciaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateTipoOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            var tipoOcorrencia = _mapper.Map<TipoOcorrencia>(request);
            await _tipoOcorrenciaRepository.InsertAsync(tipoOcorrencia);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(tipoOcorrencia.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}