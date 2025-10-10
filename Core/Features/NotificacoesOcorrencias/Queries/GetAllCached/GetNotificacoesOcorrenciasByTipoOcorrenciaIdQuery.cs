using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.NotificacoesOcorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.NotificacoesOcorrencias.Queries.GetAllCached
{
    public class GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery : IRequest<Result<List<NotificacaoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int TipoOcorrenciaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetNotificacoesOcorrenciasByTipoOcorrenciaIdQueryHandler : IRequestHandler<GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery, Result<List<NotificacaoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificacaoOcorrenciaCacheRepository _notificacaoOcorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetNotificacoesOcorrenciasByTipoOcorrenciaIdQueryHandler(INotificacaoOcorrenciaCacheRepository notificacaoOcorrenciaCache, IMapper mapper)
        {
            _notificacaoOcorrenciaCache = notificacaoOcorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificacaoOcorrenciaCachedResponse>>> Handle(GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery query, CancellationToken cancellationToken)
        {
            var notificacoesOcorrenciasList = await _notificacaoOcorrenciaCache.GetByTipoOcorrenciaIdAsync(query.TipoOcorrenciaId);
            var mappedNotificacoesOcorrencias = _mapper.Map<List<NotificacaoOcorrenciaCachedResponse>>(notificacoesOcorrenciasList);
            return Result<List<NotificacaoOcorrenciaCachedResponse>>.Success(mappedNotificacoesOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}