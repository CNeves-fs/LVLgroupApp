using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fototags.Commands.Update
{
    public class UpdateFototagCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Tag { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateFototagCommandHandler : IRequestHandler<UpdateFototagCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IFototagRepository _fototagRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateFototagCommandHandler(IFototagRepository fototagRepository, IUnitOfWork unitOfWork)
            {
                _fototagRepository = fototagRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateFototagCommand command, CancellationToken cancellationToken)
            {
                var fototag = await _fototagRepository.GetByIdAsync(command.Id);

                if (fototag == null)
                {
                    return Result<int>.Fail($"Tag de Foto Not Found.");
                }
                else
                {
                    fototag.Tag = command.Tag ?? fototag.Tag;
                    await _fototagRepository.UpdateAsync(fototag);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(fototag.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}