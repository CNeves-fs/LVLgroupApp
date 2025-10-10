using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Commands.Create
{
    public partial class CreateLojaCommand : IRequest<Result<int>>
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


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateLojaCommandHandler : IRequestHandler<CreateLojaCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaRepository _lojaRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateLojaCommandHandler(ILojaRepository lojaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _lojaRepository = lojaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateLojaCommand request, CancellationToken cancellationToken)
        {
            var loja = _mapper.Map<Loja>(request);
            await _lojaRepository.InsertAsync(loja);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(loja.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}