using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Commands.Delete
{
    public class DeleteArtigoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteArtigoCommandHandler : IRequestHandler<DeleteArtigoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IArtigoRepository _artigoRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteArtigoCommandHandler(IArtigoRepository artigoRepository, IUnitOfWork unitOfWork)
            {
                _artigoRepository = artigoRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteArtigoCommand command, CancellationToken cancellationToken)
            {
                var artigo = await _artigoRepository.GetByIdAsync(command.Id);
                await _artigoRepository.DeleteAsync(artigo);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(artigo.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}