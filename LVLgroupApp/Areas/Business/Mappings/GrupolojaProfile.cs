using AutoMapper;
using Core.Features.Gruposlojas.Commands.Create;
using Core.Features.Gruposlojas.Commands.Update;
using Core.Features.Gruposlojas.Queries.GetById;
using Core.Features.Gruposlojas.Response;
using LVLgroupApp.Areas.Business.Models.Grupoloja;

namespace LVLgroupApp.Areas.Business.Mappings
{
    internal class GrupolojaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public GrupolojaProfile()
        {
            CreateMap<GrupolojasCachedResponse, GrupolojaViewModel>().ReverseMap();
            CreateMap<CreateGrupolojaCommand, GrupolojaViewModel>().ReverseMap();
            CreateMap<UpdateGrupolojaCommand, GrupolojaViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}