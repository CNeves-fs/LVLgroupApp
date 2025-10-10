using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Commands.Update
{
    public class UpdateTipoOcorrenciaLocalizedCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateTipoOcorrenciaLocalizedCommandHandler : IRequestHandler<UpdateTipoOcorrenciaLocalizedCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly ITipoOcorrenciaLocalizedRepository _tipoOcorrenciaLocalizedRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateTipoOcorrenciaLocalizedCommandHandler(ITipoOcorrenciaLocalizedRepository tipoOcorrenciaLocalizedRepository, IUnitOfWork unitOfWork)
            {
                _tipoOcorrenciaLocalizedRepository = tipoOcorrenciaLocalizedRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateTipoOcorrenciaLocalizedCommand command, CancellationToken cancellationToken)
            {
                var tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByIdAsync(command.Id);

                if (tipoOcorrenciaLocalized == null)
                {
                    return Result<int>.Fail($"TipoOcorrenciaLocalized Not Found.");
                }
                else
                {
                    tipoOcorrenciaLocalized.TipoOcorrenciaId = command.TipoOcorrenciaId == 0 ? tipoOcorrenciaLocalized.TipoOcorrenciaId : command.TipoOcorrenciaId;
                    tipoOcorrenciaLocalized.Language = string.IsNullOrEmpty(command.Language) ? tipoOcorrenciaLocalized.Language : command.Language;
                    tipoOcorrenciaLocalized.Name = string.IsNullOrEmpty(command.Name) ? tipoOcorrenciaLocalized.Name : command.Name;
                    
                    await _tipoOcorrenciaLocalizedRepository.UpdateAsync(tipoOcorrenciaLocalized);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(tipoOcorrenciaLocalized.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}