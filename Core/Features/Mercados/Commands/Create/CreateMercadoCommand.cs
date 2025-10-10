using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Mercados.Commands.Create
{
    public partial class CreateMercadoCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeCurto { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class CreateMercadoCommandHandler : IRequestHandler<CreateMercadoCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IMercadoRepository _mercadoRepository;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            private IUnitOfWork _unitOfWork { get; set; }


            //---------------------------------------------------------------------------------------------------


            public CreateMercadoCommandHandler(IMercadoRepository mercadoRepository, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _mercadoRepository = mercadoRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(CreateMercadoCommand request, CancellationToken cancellationToken)
            {
                var mercado = _mapper.Map<Mercado>(request);
                await _mercadoRepository.InsertAsync(mercado);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(mercado.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}