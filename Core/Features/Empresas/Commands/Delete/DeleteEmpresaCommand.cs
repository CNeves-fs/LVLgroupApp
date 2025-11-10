using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Empresas.Commands.Delete
{
    public class DeleteEmpresaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public class DeleteEmpresaCommandHandler : IRequestHandler<DeleteEmpresaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IEmpresaRepository _empresaRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteEmpresaCommandHandler(IEmpresaRepository empresaRepository, IUnitOfWork unitOfWork)
            {
                _empresaRepository = empresaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteEmpresaCommand command, CancellationToken cancellationToken)
            {
                var empresa = await _empresaRepository.GetByIdAsync(command.Id);
                await _empresaRepository.DeleteAsync(empresa);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(empresa.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}