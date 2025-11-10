using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Genders.Commands.Update
{
    public class UpdateGenderCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string TamanhosNum { get; set; }

        public string TamanhosAlf { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateGenderCommandHandler : IRequestHandler<UpdateGenderCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenderRepository _genderRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateGenderCommandHandler(IGenderRepository genderRepository, IUnitOfWork unitOfWork)
            {
                _genderRepository = genderRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateGenderCommand command, CancellationToken cancellationToken)
            {
                var gender = await _genderRepository.GetByIdAsync(command.Id);

                if (gender == null)
                {
                    return Result<int>.Fail($"Gender Not Found.");
                }
                else
                {
                    gender.Nome = command.Nome ?? gender.Nome;
                    gender.TamanhosNum = command.TamanhosNum ?? gender.TamanhosNum;
                    gender.TamanhosAlf = command.TamanhosAlf ?? gender.TamanhosAlf;
                    await _genderRepository.UpdateAsync(gender);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(gender.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}