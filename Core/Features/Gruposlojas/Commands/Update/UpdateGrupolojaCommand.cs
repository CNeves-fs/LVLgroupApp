using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Gruposlojas.Commands.Update
{
    public class UpdateGrupolojaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public int EmpresaId { get; set; }

        public int MaxDiasDecisão { get; set; }




        //public string SupervisorId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateGrupolojaCommandHandler : IRequestHandler<UpdateGrupolojaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IGrupolojaRepository _grupolojaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateGrupolojaCommandHandler(IGrupolojaRepository grupolojaRepository, IUnitOfWork unitOfWork)
            {
                _grupolojaRepository = grupolojaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateGrupolojaCommand command, CancellationToken cancellationToken)
            {
                var grupoloja = await _grupolojaRepository.GetByIdAsync(command.Id);

                if (grupoloja == null)
                {
                    return Result<int>.Fail($"Grupo de lojas Not Found.");
                }
                else
                {
                    grupoloja.Nome = command.Nome ?? grupoloja.Nome;
                    grupoloja.EmpresaId = (command.EmpresaId == 0) ? grupoloja.EmpresaId : command.EmpresaId;
                    grupoloja.MaxDiasDecisão = command.MaxDiasDecisão;
                    await _grupolojaRepository.UpdateAsync(grupoloja);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(grupoloja.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}