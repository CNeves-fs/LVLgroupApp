using AutoMapper;
using Core.Entities.Business;
using Core.Features.Lojas.Commands.Create;
using Core.Features.Lojas.Commands.Update;
using Core.Features.Lojas.Response;

namespace Core.Mappings
{
    internal class LojaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public LojaProfile()
        {
            CreateMap<CreateLojaCommand, Loja>().ReverseMap();
            CreateMap<UpdateLojaCommand, Loja>().ReverseMap();
            CreateMap<LojaCachedResponse, Loja>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}