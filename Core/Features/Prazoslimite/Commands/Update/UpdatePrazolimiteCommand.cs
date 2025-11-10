using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Prazoslimite.Commands.Update
{
    public class UpdatePrazolimiteCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Alarme { get; set; }

        public int LimiteMin { get; set; }

        public int LimiteMax { get; set; }

        public string Cortexto { get; set; }

        public string Corfundo { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdatePrazolimiteCommandHandler : IRequestHandler<UpdatePrazolimiteCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IPrazolimiteRepository _prazolimiteRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdatePrazolimiteCommandHandler(IPrazolimiteRepository prazolimiteRepository, IUnitOfWork unitOfWork)
            {
                _prazolimiteRepository = prazolimiteRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdatePrazolimiteCommand command, CancellationToken cancellationToken)
            {
                var prazolimite = await _prazolimiteRepository.GetByIdAsync(command.Id);

                if (prazolimite == null)
                {
                    return Result<int>.Fail($"Tag de Foto Not Found.");
                }
                else
                {
                    prazolimite.Alarme = command.Alarme ?? prazolimite.Alarme;
                    prazolimite.LimiteMax = command.LimiteMax;
                    prazolimite.LimiteMin = command.LimiteMin;
                    prazolimite.Cortexto = command.Cortexto ?? prazolimite.Cortexto;
                    prazolimite.Corfundo = command.Corfundo ?? prazolimite.Corfundo;
                    await _prazolimiteRepository.UpdateAsync(prazolimite);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(prazolimite.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}