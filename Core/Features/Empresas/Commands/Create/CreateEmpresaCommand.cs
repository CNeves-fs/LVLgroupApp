using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Empresas.Commands.Create
{
    public partial class CreateEmpresaCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeCurto { get; set; }

        public byte[] LogoPicture { get; set; }


        //---------------------------------------------------------------------------------------------------

    }

    public class CreateEmpresaCommandHandler : IRequestHandler<CreateEmpresaCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IEmpresaRepository _empresaRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateEmpresaCommandHandler(IEmpresaRepository empresaRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _empresaRepository = empresaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateEmpresaCommand request, CancellationToken cancellationToken)
        {
            var empresa = _mapper.Map<Empresa>(request);
            await _empresaRepository.InsertAsync(empresa);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(empresa.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}