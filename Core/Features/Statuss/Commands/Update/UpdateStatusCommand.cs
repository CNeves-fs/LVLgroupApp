using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Core.Features.Statuss.Commands.Update
{
    public class UpdateStatusCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int Tipo { get; set; }

        public string Texto { get; set; }

        public string Cortexto { get; set; }

        public string Corfundo { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IStatusRepository _statusRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateStatusCommandHandler(IStatusRepository statusRepository, IUnitOfWork unitOfWork)
            {
                _statusRepository = statusRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateStatusCommand command, CancellationToken cancellationToken)
            {
                var status = await _statusRepository.GetByIdAsync(command.Id);

                if (status == null)
                {
                    return Result<int>.Fail($"Tag de Status Not Found.");
                }
                else
                {
                    status.Tipo = (command.Tipo == 0) ? status.Tipo : command.Tipo;
                    status.Texto = command.Texto ?? status.Texto;
                    status.Cortexto = command.Cortexto ?? status.Cortexto;
                    status.Corfundo = command.Corfundo ?? status.Corfundo;
                    await _statusRepository.UpdateAsync(status);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(status.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}