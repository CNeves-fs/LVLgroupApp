using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Extensions;
using Core.Features.TiposOcorrenciasLocalized.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrenciasLocalized.Queries.GetAllPaged
{
    public class GetAllTiposOcorrenciasLocalizedQuery : IRequest<PaginatedResult<TipoOcorrenciaLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorrenciasLocalizedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllTiposOcorrenciasLocalizedQueryHandler : IRequestHandler<GetAllTiposOcorrenciasLocalizedQuery, PaginatedResult<TipoOcorrenciaLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaLocalizedRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorrenciasLocalizedQueryHandler(ITipoOcorrenciaLocalizedRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<TipoOcorrenciaLocalizedCachedResponse>> Handle(GetAllTiposOcorrenciasLocalizedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<TipoOcorrenciaLocalized, TipoOcorrenciaLocalizedCachedResponse>> expression = tol => new TipoOcorrenciaLocalizedCachedResponse
            {
                Id = tol.Id,
                TipoOcorrenciaId = tol.TipoOcorrenciaId,
                Language = tol.Language,
                Name = tol.Name
            };
            var paginatedList = await _repository.TiposOcorrenciasLocalized
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}