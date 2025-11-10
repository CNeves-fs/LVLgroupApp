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
    public class GetAllNotificacoesOcorenciasCachedQuery : IRequest<Result<List<NotificacaoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllNotificacoesOcorenciasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllNotificacoesOcorrenciasCachedQueryHandler : IRequestHandler<GetAllNotificacoesOcorenciasCachedQuery, Result<List<NotificacaoOcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly INotificacaoOcorrenciaCacheRepository _notificacaoOcorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllNotificacoesOcorrenciasCachedQueryHandler(INotificacaoOcorrenciaCacheRepository notificacaoOcorrenciaCache, IMapper mapper)
        {
            _notificacaoOcorrenciaCache = notificacaoOcorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<NotificacaoOcorrenciaCachedResponse>>> Handle(GetAllNotificacoesOcorenciasCachedQuery request, CancellationToken cancellationToken)
        {
            var notificacaoOcorrenciaList = await _notificacaoOcorrenciaCache.GetCachedListAsync();
            var mappedNotificacoesOcorrencias = _mapper.Map<List<NotificacaoOcorrenciaCachedResponse>>(notificacaoOcorrenciaList);
            return Result<List<NotificacaoOcorrenciaCachedResponse>>.Success(mappedNotificacoesOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}