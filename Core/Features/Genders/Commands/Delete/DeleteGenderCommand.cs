using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Commands.Delete
{
    public class DeleteGenderCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class DeleteGenderCommandHandler : IRequestHandler<DeleteGenderCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IGenderRepository _genderRepository;
            private readonly IUnitOfWork _unitOfWork;


            //---------------------------------------------------------------------------------------------------


            public DeleteGenderCommandHandler(IGenderRepository genderRepository, IUnitOfWork unitOfWork)
            {
                _genderRepository = genderRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(DeleteGenderCommand command, CancellationToken cancellationToken)
            {
                var gender = await _genderRepository.GetByIdAsync(command.Id);
                await _genderRepository.DeleteAsync(gender);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(gender.Id);
            }


            //---------------------------------------------------------------------------------------------------

        }

        //---------------------------------------------------------------------------------------------------

    }
}