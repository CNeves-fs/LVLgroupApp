using AutoMapper;
using Core.Features.Lojas.Commands.Create;
using Core.Features.Lojas.Commands.Update;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Lojas.Response;
using LVLgroupApp.Areas.Business.Models.Loja;

namespace LVLgroupApp.Areas.Business.Mappings
{
    internal class LojaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public LojaProfile()
        {
            CreateMap<LojaCachedResponse, LojaViewModel>().ReverseMap();
            CreateMap<CreateLojaCommand, LojaViewModel>().ReverseMap();
            CreateMap<UpdateLojaCommand, LojaViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}