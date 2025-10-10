using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Commands.Update
{
    public class UpdateTipoOcorrenciaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string DefaultName { get; set; }

        public int CategoriaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateTipoOcorrenciaCommandHandler : IRequestHandler<UpdateTipoOcorrenciaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly ITipoOcorrenciaRepository _tipoOcorrenciaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateTipoOcorrenciaCommandHandler(ITipoOcorrenciaRepository tipoOcorrenciaRepository, IUnitOfWork unitOfWork)
            {
                _tipoOcorrenciaRepository = tipoOcorrenciaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateTipoOcorrenciaCommand command, CancellationToken cancellationToken)
            {
                var tipoOcorrencia = await _tipoOcorrenciaRepository.GetByIdAsync(command.Id);

                if (tipoOcorrencia == null)
                {
                    return Result<int>.Fail($"TipoOcorrencia Not Found.");
                }
                else
                {
                    tipoOcorrencia.DefaultName = string.IsNullOrEmpty(command.DefaultName) ? tipoOcorrencia.DefaultName : command.DefaultName;
                    tipoOcorrencia.CategoriaId = (command.CategoriaId == 0) ? tipoOcorrencia.CategoriaId : command.CategoriaId;

                    await _tipoOcorrenciaRepository.UpdateAsync(tipoOcorrencia);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(tipoOcorrencia.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}