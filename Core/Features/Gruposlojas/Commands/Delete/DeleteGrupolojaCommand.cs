using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Gruposlojas.Commands.Delete
{
    public class DeleteGrupolojaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteGrupolojaCommandHandler : IRequestHandler<DeleteGrupolojaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IGrupolojaRepository _grupolojaRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteGrupolojaCommandHandler(IGrupolojaRepository grupolojaRepository, IUnitOfWork unitOfWork)
            {
                _grupolojaRepository = grupolojaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteGrupolojaCommand command, CancellationToken cancellationToken)
            {
                var grupoloja = await _grupolojaRepository.GetByIdAsync(command.Id);
                await _grupolojaRepository.DeleteAsync(grupoloja);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(grupoloja.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}