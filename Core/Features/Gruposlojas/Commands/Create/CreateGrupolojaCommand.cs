using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Gruposlojas.Commands.Create
{
    public partial class CreateGrupolojaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        public string Nome { get; set; }

        public int EmpresaId { get; set; }

        public int MaxDiasDecisão { get; set; }




        //public string SupervisorId { get; set; }
    }


    //---------------------------------------------------------------------------------------------------


    public class CreateGrupolojaCommandHandler : IRequestHandler<CreateGrupolojaCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGrupolojaRepository _grupolojaRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateGrupolojaCommandHandler(IGrupolojaRepository grupolojaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _grupolojaRepository = grupolojaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateGrupolojaCommand request, CancellationToken cancellationToken)
        {
            var grupoloja = _mapper.Map<Grupoloja>(request);
            await _grupolojaRepository.InsertAsync(grupoloja);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(grupoloja.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}