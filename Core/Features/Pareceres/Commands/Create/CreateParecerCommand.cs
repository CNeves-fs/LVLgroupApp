using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Pareceres.Commands.Create
{
    public partial class CreateParecerCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Email { get; set; }

        public string Opinião { get; set; }


    }


    //---------------------------------------------------------------------------------------------------


    public class CreateParecerCommandHandler : IRequestHandler<CreateParecerCommand, Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IParecerRepository _parecerRepository;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        private IUnitOfWork _unitOfWork { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CreateParecerCommandHandler(IParecerRepository parecerRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _parecerRepository = parecerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<int>> Handle(CreateParecerCommand request, CancellationToken cancellationToken)
        {
            var parecer = _mapper.Map<Parecer>(request);
            await _parecerRepository.InsertAsync(parecer);
            await _unitOfWork.Commit(cancellationToken);
            return Result<int>.Success(parecer.Id);
        }


        //---------------------------------------------------------------------------------------------------

    }
}