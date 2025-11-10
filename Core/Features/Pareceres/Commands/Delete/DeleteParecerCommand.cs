using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Pareceres.Commands.Delete
{
    public class DeleteParecerCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteParecerCommandHandler : IRequestHandler<DeleteParecerCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IParecerRepository _parecerRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteParecerCommandHandler(IParecerRepository parecerRepository, IUnitOfWork unitOfWork)
            {
                _parecerRepository = parecerRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteParecerCommand command, CancellationToken cancellationToken)
            {
                var parecer = await _parecerRepository.GetByIdAsync(command.Id);
                await _parecerRepository.DeleteAsync(parecer);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(parecer.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}