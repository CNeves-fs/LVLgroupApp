using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Empresas.Commands.Update
{
    public class UpdateEmpresaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeCurto { get; set; }

        public byte[] LogoPicture { get; set; }


        public class UpdateEmpresaCommandHandler : IRequestHandler<UpdateEmpresaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmpresaRepository _empresaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateEmpresaCommandHandler(IEmpresaRepository empresaRepository, IUnitOfWork unitOfWork)
            {
                _empresaRepository = empresaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateEmpresaCommand command, CancellationToken cancellationToken)
            {
                var empresa = await _empresaRepository.GetByIdAsync(command.Id);

                if (empresa == null)
                {
                    return Result<int>.Fail($"Empresa Not Found.");
                }
                else
                {
                    empresa.Nome = command.Nome ?? empresa.Nome;
                    empresa.NomeCurto = command.NomeCurto ?? empresa.NomeCurto;
                    empresa.LogoPicture = (command.LogoPicture == null || command.LogoPicture.Length == 0) ? empresa.LogoPicture : command.LogoPicture;
                    await _empresaRepository.UpdateAsync(empresa);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(empresa.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}