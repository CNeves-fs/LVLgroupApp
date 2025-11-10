using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fotos.Commands.Update
{
    public class UpdateFotoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string FileName { get; set; }

        public string ClaimFolder { get; set; }     // se null => file in Claim.CodeId folder

        public string Path { get; set; }                // path no server /wwwroot/Claims/20221022-GEX-LLLL-XXXX/filename

        public int? FototagId { get; set; }

        public string Descrição { get; set; }

        public int? ClaimId { get; set; }          // null => Claim a ser criada ainda não tem Id


        //---------------------------------------------------------------------------------------------------


        public class UpdateFotoCommandHandler : IRequestHandler<UpdateFotoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IFotoRepository _fotoRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateFotoCommandHandler(IFotoRepository fotoRepository, IUnitOfWork unitOfWork)
            {
                _fotoRepository = fotoRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateFotoCommand command, CancellationToken cancellationToken)
            {
                var foto = await _fotoRepository.GetByIdAsync(command.Id);

                if (foto == null)
                {
                    return Result<int>.Fail($"Foto Not Found.");
                }
                else
                {
                    foto.FileName = command.FileName ?? foto.FileName;
                    foto.TempFolderGuid = command.ClaimFolder;
                    foto.FototagId = (command.FototagId == null) ? foto.FototagId : command.FototagId;
                    foto.Path = command.Path ?? foto.Path;
                    foto.Descrição = command.Descrição ?? foto.Descrição;
                    foto.ClaimId = (command.ClaimId == 0) ? foto.ClaimId : command.ClaimId;
                    await _fotoRepository.UpdateAsync(foto);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(foto.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}