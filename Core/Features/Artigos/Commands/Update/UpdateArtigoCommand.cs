using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Commands.Update
{
    public class UpdateArtigoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Referencia { get; set; }

        public int EmpresaId { get; set; }

        public int GenderId { get; set; }

        public string Pele { get; set; }

        public string Cor { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateArtigoCommandHandler : IRequestHandler<UpdateArtigoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IArtigoRepository _artigoRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateArtigoCommandHandler(IArtigoRepository artigoRepository, IUnitOfWork unitOfWork)
            {
                _artigoRepository = artigoRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateArtigoCommand command, CancellationToken cancellationToken)
            {
                var artigo = await _artigoRepository.GetByIdAsync(command.Id);

                if (artigo == null)
                {
                    return Result<int>.Fail($"Artigo Not Found.");
                }
                else
                {
                    artigo.Referencia = command.Referencia ?? artigo.Referencia;
                    artigo.EmpresaId = (command.EmpresaId == 0) ? artigo.EmpresaId : command.EmpresaId;
                    artigo.GenderId = (command.GenderId == 0) ? artigo.GenderId : command.GenderId;
                    artigo.Pele = command.Pele ?? artigo.Pele;
                    artigo.Cor = command.Cor ?? artigo.Cor;

                    await _artigoRepository.UpdateAsync(artigo);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(artigo.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}