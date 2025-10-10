using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Commands.Create
{
    public partial class CreateArtigoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        public string Referencia { get; set; }

        public int EmpresaId { get; set; }

        public int GenderId { get; set; }

        public string Pele { get; set; }

        public string Cor { get; set; }
    }


    //---------------------------------------------------------------------------------------------------


    public class CreateArtigoCommandHandler : IRequestHandler<CreateArtigoCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IArtigoRepository _artigoRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateArtigoCommandHandler(IArtigoRepository artigoRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _artigoRepository = artigoRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateArtigoCommand request, CancellationToken cancellationToken)
        {
            var artigo = _mapper.Map<Artigo>(request);
            await _artigoRepository.InsertAsync(artigo);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(artigo.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}