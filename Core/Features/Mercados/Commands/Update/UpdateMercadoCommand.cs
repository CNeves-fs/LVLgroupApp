using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Mercados.Commands.Update
{
    public class UpdateMercadoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeCurto { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateMercadoCommandHandler : IRequestHandler<UpdateMercadoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;

            private readonly IMercadoRepository _mercadoRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateMercadoCommandHandler(IMercadoRepository mercadoRepository, IUnitOfWork unitOfWork)
            {
                _mercadoRepository = mercadoRepository;

                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateMercadoCommand command, CancellationToken cancellationToken)
            {
                var mercado = await _mercadoRepository.GetByIdAsync(command.Id);

                if (mercado == null)
                {
                    return Result<int>.Fail($"Mercado Not Found.");
                }
                else
                {
                    mercado.Nome = command.Nome ?? mercado.Nome;
                    mercado.NomeCurto = command.NomeCurto ?? mercado.NomeCurto;
                    await _mercadoRepository.UpdateAsync(mercado);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(mercado.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }

    }
}