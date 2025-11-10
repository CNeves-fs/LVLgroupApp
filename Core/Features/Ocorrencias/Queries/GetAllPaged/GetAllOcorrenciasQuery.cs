using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Extensions;
using Core.Features.Ocorrencias.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Queries.GetAllPaged
{
    public class GetAllOcorrenciasQuery : IRequest<PaginatedResult<OcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciasQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllOcorrenciasQueryHandler : IRequestHandler<GetAllOcorrenciasQuery, PaginatedResult<OcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciasQueryHandler(IOcorrenciaRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<OcorrenciaCachedResponse>> Handle(GetAllOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Ocorrencia, OcorrenciaCachedResponse>> expression = o => new OcorrenciaCachedResponse
            {
                Id = o.Id,
                TipoOcorrenciaId = o.TipoOcorrenciaId,
                DataOcorrencia = o.DataOcorrencia,
                Descrição = o.Descrição,
                Comentário = o.Comentário,
                EmpresaId = o.EmpresaId,
                GrupolojaId = o.GrupolojaId,
                LojaId = o.LojaId,
                MercadoId = o.MercadoId
            };
            var paginatedList = await _repository.Ocorrencias
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}