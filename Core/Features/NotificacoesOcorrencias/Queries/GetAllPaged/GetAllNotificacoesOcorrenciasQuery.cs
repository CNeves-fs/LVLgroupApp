using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Extensions;
using Core.Features.NotificacoesOcorrencias.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificacoesOcorrencias.Queries.GetAllPaged
{
    public class GetAllNotificacoesOcorrenciasQuery : IRequest<PaginatedResult<NotificacaoOcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllNotificacoesOcorrenciasQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllNotificacoesOcorrenciasQueryHandler : IRequestHandler<GetAllNotificacoesOcorrenciasQuery, PaginatedResult<NotificacaoOcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificacaoOcorrenciaRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllNotificacoesOcorrenciasQueryHandler(INotificacaoOcorrenciaRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<NotificacaoOcorrenciaCachedResponse>> Handle(GetAllNotificacoesOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<NotificacaoOcorrencia, NotificacaoOcorrenciaCachedResponse>> expression = no => new NotificacaoOcorrenciaCachedResponse
            {
                Id = no.Id,
                TipoOcorrenciaId = no.TipoOcorrenciaId,
                TipoDestino = no.TipoDestino,
                ApplicationUserId = no.ApplicationUserId,
                ApplicationUserEmail = no.ApplicationUserEmail
            };
            var paginatedList = await _repository.NotificacoesOcorrencia
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}