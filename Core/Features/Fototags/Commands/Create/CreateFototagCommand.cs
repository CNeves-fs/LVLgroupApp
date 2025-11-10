using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fototags.Commands.Create
{
    public partial class CreateFototagCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------

        public int Id { get; set; }

        public string Tag { get; set; }
    }


    //---------------------------------------------------------------------------------------------------


    public class CreateFototagCommandHandler : IRequestHandler<CreateFototagCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFototagRepository _fototagRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateFototagCommandHandler(IFototagRepository fototagRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _fototagRepository = fototagRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateFototagCommand request, CancellationToken cancellationToken)
        {
            var fototag = _mapper.Map<Fototag>(request);
            await _fototagRepository.InsertAsync(fototag);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(fototag.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}