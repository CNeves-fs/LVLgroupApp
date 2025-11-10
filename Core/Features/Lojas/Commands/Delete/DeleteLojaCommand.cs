using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Commands.Delete
{
    public class DeleteLojaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteLojaCommandHandler : IRequestHandler<DeleteLojaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ILojaRepository _lojaRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteLojaCommandHandler(ILojaRepository lojaRepository, IUnitOfWork unitOfWork)
            {
                _lojaRepository = lojaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteLojaCommand command, CancellationToken cancellationToken)
            {
                var loja = await _lojaRepository.GetByIdAsync(command.Id);
                await _lojaRepository.DeleteAsync(loja);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(loja.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}