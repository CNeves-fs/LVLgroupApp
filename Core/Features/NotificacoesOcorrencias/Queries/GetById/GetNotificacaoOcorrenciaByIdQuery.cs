using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.NotificacoesOcorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Queries.GetById
{
    public class GetNotificacaoOcorrenciaByIdQuery : IRequest<Result<NotificacaoOcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetNotificacaoOcorrenciaByIdQueryHandler : IRequestHandler<GetNotificacaoOcorrenciaByIdQuery, Result<NotificacaoOcorrenciaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly INotificacaoOcorrenciaCacheRepository _notificacaoOcorrenciaCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetNotificacaoOcorrenciaByIdQueryHandler(INotificacaoOcorrenciaCacheRepository notificacaoOcorrenciaCache, IMapper mapper)
            {
                _notificacaoOcorrenciaCache = notificacaoOcorrenciaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<NotificacaoOcorrenciaCachedResponse>> Handle(GetNotificacaoOcorrenciaByIdQuery query, CancellationToken cancellationToken)
            {
                var notificacaoOcorrencia = await _notificacaoOcorrenciaCache.GetByIdAsync(query.Id);
                var mappedNotificacaoOcorrencia = _mapper.Map<NotificacaoOcorrenciaCachedResponse>(notificacaoOcorrencia);
                return Result<NotificacaoOcorrenciaCachedResponse>.Success(mappedNotificacaoOcorrencia);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}