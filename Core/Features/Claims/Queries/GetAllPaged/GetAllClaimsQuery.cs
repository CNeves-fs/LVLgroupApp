using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Claims.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Core.Features.Claims.Queries.GetAllPaged
{
    public class GetAllClaimsQuery : IRequest<PaginatedResult<ClaimCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllClaimsQueryHandler : IRequestHandler<GetAllClaimsQuery, PaginatedResult<ClaimCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsQueryHandler(IClaimRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ClaimCachedResponse>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Claim, ClaimCachedResponse>> expression = r => new ClaimCachedResponse
            {
                Id = r.Id,
                CodeId = r.CodeId,
                StatusId = r.StatusId,
                LojaId = (int) r.LojaId,
                MotivoClaim = r.MotivoClaim,
                DataClaim = r.DataClaim,
                DataLimite = r.DataLimite,
                EmailAutor = r.EmailAutor,

                ArtigoId = r.ArtigoId,
                Tamanho = r.Tamanho,
                DataCompra = (DateTime) r.DataCompra,                
                DefeitoDoArtigo = r.DefeitoDoArtigo,

                ClienteId = r.ClienteId,

                TotalFotos = r.TotalFotos,

                ParecerResponsavelId = r.ParecerResponsavelId,
                ParecerColaboradorId = r.ParecerColaboradorId,
                ParecerGerenteLojaId = r.ParecerGerenteLojaId,
                ParecerSupervisorId = r.ParecerSupervisorId,
                ParecerRevisorId = r.ParecerRevisorId,
                ParecerAdministraçãoId = r.ParecerAdministraçãoId,

                DecisãoFinal = r.DecisãoFinal,
                Rejeitada = r.Rejeitada,
                Trocadireta = r.Trocadireta,
                DevoluçãoDinheiro = r.DevoluçãoDinheiro,
                Reparação = r.Reparação,
                NotaCrédito = r.NotaCrédito,
                DataDecisão = r.DataDecisão,
                EmailAutorDecisão = r.EmailAutorDecisão,

                ObservaçõesFecho = r.ObservaçõesFecho,
                DataFecho = r.DataFecho,
                EmailAutorFechoEmLoja = r.EmailAutorFechoEmLoja,

                MaxDiasDecisão = r.MaxDiasDecisão,
                FechoClaimEmLoja = r.FechoClaimEmLoja

    };
            var paginatedList = await _repository.Claims
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}