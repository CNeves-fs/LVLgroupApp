using AutoMapper;
using Core.Entities.Artigos;
using Core.Features.Artigos.Commands.Create;
using Core.Features.Artigos.Queries.GetAllCached;
using Core.Features.Artigos.Queries.GetAllPaged;
using Core.Features.Artigos.Queries.GetById;
using Core.Features.Artigos.Queries.GetByRef;
using Core.Features.Artigos.Response;

namespace Core.Mappings
{
    internal class ArtigoProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ArtigoProfile()
        {
            CreateMap<CreateArtigoCommand, Artigo>().ReverseMap();
            CreateMap<ArtigoCachedResponse, Artigo>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}