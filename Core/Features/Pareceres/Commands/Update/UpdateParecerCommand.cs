using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Pareceres.Commands.Update
{
    public class UpdateParecerCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Email { get; set; }

        public string Opinião { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateParecerCommandHandler : IRequestHandler<UpdateParecerCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IParecerRepository _parecerRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateParecerCommandHandler(IParecerRepository parecerRepository, IUnitOfWork unitOfWork)
            {
                _parecerRepository = parecerRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateParecerCommand command, CancellationToken cancellationToken)
            {
                var parecer = await _parecerRepository.GetByIdAsync(command.Id);

                if (parecer == null)
                {
                    return Result<int>.Fail($"Parecer Not Found.");
                }
                else
                {
                    parecer.Email = command.Email ?? parecer.Email;
                    parecer.Data = command.Data;
                    parecer.Opinião = command.Opinião ?? parecer.Opinião;
                    await _parecerRepository.UpdateAsync(parecer);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(parecer.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}