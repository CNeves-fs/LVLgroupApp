using AutoMapper;
using Core.Features.Artigos.Commands.Create;
using Core.Features.Artigos.Commands.Update;
using Core.Features.Artigos.Queries.GetAllCached;
using Core.Features.Artigos.Queries.GetById;
using Core.Features.Artigos.Queries.GetByRef;
using Core.Features.Artigos.Response;
using LVLgroupApp.Areas.Artigo.Models.Artigo;

namespace LVLgroupApp.Areas.Artigo.Mappings
{
    internal class ArtigoProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ArtigoProfile()
        {
            CreateMap<ArtigoCachedResponse, ArtigoViewModel>().ReverseMap();
            CreateMap<CreateArtigoCommand, ArtigoViewModel>().ReverseMap();
            CreateMap<UpdateArtigoCommand, ArtigoViewModel>().ReverseMap();

            CreateMap<ArtigoCachedResponse, TabArtigoViewModel>().ReverseMap();
            CreateMap<CreateArtigoCommand, TabArtigoViewModel>().ReverseMap();
            CreateMap<UpdateArtigoCommand, TabArtigoViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}