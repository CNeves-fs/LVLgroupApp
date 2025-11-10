using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Artigos;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Commands.Create
{
    public partial class CreateGenderCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        public string Nome { get; set; }

        public string TamanhosNum { get; set; }

        public string TamanhosAlf { get; set; }
    }


    //---------------------------------------------------------------------------------------------------


    public class CreateGenderCommandHandler : IRequestHandler<CreateGenderCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGenderRepository _genderRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateGenderCommandHandler(IGenderRepository genderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _genderRepository = genderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
        {
            var gender = _mapper.Map<Gender>(request);
            await _genderRepository.InsertAsync(gender);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(gender.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}