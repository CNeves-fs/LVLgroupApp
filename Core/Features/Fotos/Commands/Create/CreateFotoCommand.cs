using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fotos.Commands.Create
{
    public partial class CreateFotoCommand : IRequest<Result<int>>
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

    }


    //---------------------------------------------------------------------------------------------------


    public class CreateFotoCommandHandler : IRequestHandler<CreateFotoCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFotoRepository _fotoRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateFotoCommandHandler(IFotoRepository fotoRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _fotoRepository = fotoRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateFotoCommand request, CancellationToken cancellationToken)
        {
            var foto = _mapper.Map<Foto>(request);
            await _fotoRepository.InsertAsync(foto);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(foto.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}