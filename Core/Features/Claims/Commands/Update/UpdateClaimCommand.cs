using AspNetCoreHero.Results;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Claims.Commands.Update
{
    public class UpdateClaimCommand : IRequest<Result<int>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-EEE-LLLL-XXXX

        public DateTime DataClaim { get; set; }

        public DateTime DataEntradaSistemaClaim { get; set; }

        public int StatusId { get; set; }

        public int PreviousStatusId { get; set; }

        public string MotivoClaim { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public DateTime DataLimite { get; set; }

        public string EmailAutor { get; set; }

        public string DecisãoFinal { get; set; }

        public bool Rejeitada { get; set; }

        public bool Trocadireta { get; set; }

        public bool DevoluçãoDinheiro { get; set; }

        public bool Reparação { get; set; }

        public bool NotaCrédito { get; set; }

        public DateTime DataDecisão { get; set; }

        public string EmailAutorDecisão { get; set; }

        public string ObservaçõesFecho { get; set; }

        public DateTime DataFecho { get; set; }

        public string EmailAutorFechoEmLoja { get; set; }

        // Tab Artigo

        public int? ArtigoId { get; set; }

        public string Tamanho { get; set; }

        public int TamanhoId { get; set; }

        public DateTime DataCompra { get; set; }

        public double TempoCompra { get; set; }

        public string DefeitoDoArtigo { get; set; }

        // Tab Opiniões

        public int? ParecerResponsavelId { get; set; }

        public int? ParecerColaboradorId { get; set; }

        public int? ParecerGerenteLojaId { get; set; }

        public int? ParecerSupervisorId { get; set; }

        public int? ParecerRevisorId { get; set; }

        public int? ParecerAdministraçãoId { get; set; }

        // Tab Fotos

        public int TotalFotos { get; set; }

        // Tab Cliente

        public int ClienteId { get; set; }

        public int MaxDiasDecisão { get; set; }

        public bool FechoClaimEmLoja { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class UpdateClaimCommandHandler : IRequestHandler<UpdateClaimCommand, Result<int>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IUnitOfWork _unitOfWork;
            private readonly IClaimRepository _claimRepository;


            //---------------------------------------------------------------------------------------------------


            public UpdateClaimCommandHandler(IClaimRepository claimRepository, IUnitOfWork unitOfWork)
            {
                _claimRepository = claimRepository;
                _unitOfWork = unitOfWork;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<int>> Handle(UpdateClaimCommand command, CancellationToken cancellationToken)
            {
                var claim = await _claimRepository.GetByIdAsync(command.Id);

                if (claim == null)
                {
                    return Result<int>.Fail($"Claim Not Found.");
                }
                else
                {
                    claim.CodeId = command.CodeId ?? claim.CodeId;
                    claim.StatusId = (command.StatusId == 0) ? claim.StatusId : command.StatusId;
                    claim.PreviousStatusId = (command.PreviousStatusId == 0) ? claim.PreviousStatusId : command.PreviousStatusId;
                    claim.EmpresaId = (command.EmpresaId == 0) ? claim.EmpresaId : command.EmpresaId;
                    claim.GrupolojaId = (command.GrupolojaId == 0) ? claim.GrupolojaId : command.GrupolojaId;
                    claim.LojaId = (command.LojaId == 0) ? claim.LojaId : command.LojaId;
                    claim.MotivoClaim = command.MotivoClaim ?? claim.MotivoClaim;
                    claim.DataClaim = command.DataClaim;
                    claim.DataLimite = command.DataLimite;
                    claim.EmailAutor = command.EmailAutor ?? claim.EmailAutor;

                    claim.ArtigoId = (command.ArtigoId == 0) ? claim.ArtigoId : command.ArtigoId;
                    claim.Tamanho = command.Tamanho ?? claim.Tamanho;
                    claim.TamanhoId = (command.TamanhoId == 0) ? claim.TamanhoId : command.TamanhoId;
                    claim.DefeitoDoArtigo = command.DefeitoDoArtigo ?? claim.DefeitoDoArtigo;
                    claim.DataCompra = command.DataCompra;

                    claim.TotalFotos = (command.TotalFotos == 0) ? claim.TotalFotos : command.TotalFotos;

                    claim.ClienteId = (command.ClienteId == 0) ? claim.ClienteId : command.ClienteId;

                    claim.ParecerColaboradorId = (command.ParecerColaboradorId == null) ? claim.ParecerColaboradorId : command.ParecerColaboradorId;
                    claim.ParecerGerenteLojaId = (command.ParecerGerenteLojaId == null) ? claim.ParecerGerenteLojaId : command.ParecerGerenteLojaId;
                    claim.ParecerSupervisorId = (command.ParecerSupervisorId == null) ? claim.ParecerSupervisorId : command.ParecerSupervisorId;
                    claim.ParecerRevisorId = (command.ParecerRevisorId == null) ? claim.ParecerRevisorId : command.ParecerRevisorId;
                    claim.ParecerAdministraçãoId = (command.ParecerAdministraçãoId == null) ? claim.ParecerAdministraçãoId : command.ParecerAdministraçãoId;

                    claim.DecisãoFinal = command.DecisãoFinal ?? claim.DecisãoFinal;
                    claim.Rejeitada = command.Rejeitada;
                    claim.Trocadireta = command.Trocadireta;
                    claim.DevoluçãoDinheiro = command.DevoluçãoDinheiro;
                    claim.Reparação = command.Reparação;
                    claim.NotaCrédito = command.NotaCrédito;
                    claim.DataDecisão = command.DataDecisão;
                    claim.EmailAutorDecisão = command.EmailAutorDecisão ?? claim.EmailAutorDecisão;

                    claim.ObservaçõesFecho = command.ObservaçõesFecho ?? claim.ObservaçõesFecho;
                    claim.DataFecho = command.DataFecho;
                    claim.EmailAutorFechoEmLoja = command.EmailAutorFechoEmLoja ?? claim.EmailAutorFechoEmLoja;

                    claim.MaxDiasDecisão = (command.MaxDiasDecisão == 0) ? claim.MaxDiasDecisão : command.MaxDiasDecisão;
                    claim.FechoClaimEmLoja = command.FechoClaimEmLoja;

                    await _claimRepository.UpdateAsync(claim);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(claim.Id);
                }
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}