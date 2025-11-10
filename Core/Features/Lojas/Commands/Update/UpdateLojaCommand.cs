using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Commands.Update
{
    public class UpdateLojaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeCurto { get; set; }

        public string Cidade { get; set; }

        public string País { get; set; }

        public int GrupolojaId { get; set; }

        public int? MercadoId { get; set; }

        public bool FechoClaimEmLoja { get; set; }

        public bool Active { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateLojaCommandHandler : IRequestHandler<UpdateLojaCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly ILojaRepository _lojaRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateLojaCommandHandler(ILojaRepository lojaRepository, IUnitOfWork unitOfWork)
            {
                _lojaRepository = lojaRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateLojaCommand command, CancellationToken cancellationToken)
            {
                var loja = await _lojaRepository.GetByIdAsync(command.Id);

                if (loja == null)
                {
                    return Result<int>.Fail($"Loja Not Found.");
                }
                else
                {
                    loja.Nome = command.Nome ?? loja.Nome;
                    loja.NomeCurto = command.NomeCurto ?? loja.NomeCurto;
                    loja.Cidade = command.Cidade ?? loja.Cidade;
                    loja.País = command.País ?? loja.País;
                    loja.GrupolojaId = (command.GrupolojaId == 0) ? loja.GrupolojaId : command.GrupolojaId;
                    loja.MercadoId = (command.MercadoId == 0) ? loja.MercadoId : command.MercadoId;
                    loja.FechoClaimEmLoja = command.FechoClaimEmLoja;
                    loja.Active = command.Active;
                    //loja.GerenteId = command.GerenteId ?? loja.GerenteId;
                    //loja.BasicLojaId = command.BasicLojaId ?? loja.BasicLojaId;
                    await _lojaRepository.UpdateAsync(loja);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(loja.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}