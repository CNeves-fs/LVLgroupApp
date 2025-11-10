using AutoMapper;
using Core.Features.Genders.Commands.Create;
using Core.Features.Genders.Commands.Update;
using Core.Features.Genders.Queries.GetById;
using Core.Features.Genders.Queries.GetByNome;
using Core.Features.Genders.Response;
using LVLgroupApp.Areas.Artigo.Models.Gender;

namespace LVLgroupApp.Areas.Artigo.Mappings
{
    internal class GenderProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public GenderProfile()
        {
            CreateMap<GenderCachedResponse, GenderViewModel>().ReverseMap();
            CreateMap<CreateGenderCommand, GenderViewModel>().ReverseMap();
            CreateMap<UpdateGenderCommand, GenderViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}