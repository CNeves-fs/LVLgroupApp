using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Prazoslimite.Commands.Delete
{
    public class DeletePrazolimiteCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeletePrazolimiteCommandHandler : IRequestHandler<DeletePrazolimiteCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IPrazolimiteRepository _prazolimiteRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeletePrazolimiteCommandHandler(IPrazolimiteRepository prazolimiteRepository, IUnitOfWork unitOfWork)
            {
                _prazolimiteRepository = prazolimiteRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeletePrazolimiteCommand command, CancellationToken cancellationToken)
            {
                var prazolimite = await _prazolimiteRepository.GetByIdAsync(command.Id);
                await _prazolimiteRepository.DeleteAsync(prazolimite);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(prazolimite.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}