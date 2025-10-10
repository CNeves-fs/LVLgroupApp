using AutoMapper;
using Core.Features.Pareceres.Commands.Create;
using Core.Features.Pareceres.Commands.Update;
using Core.Features.Pareceres.Queries.GetAllCached;
using Core.Features.Pareceres.Queries.GetById;
using Core.Features.Pareceres.Response;
using LVLgroupApp.Areas.Claim.Models.ParecerTécnico;

namespace LVLgroupApp.Areas.Claim.Mappings
{
    internal class ParecerProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ParecerProfile()
        {
            CreateMap<ParecerCachedResponse, ParecerViewModel>().ReverseMap();
            CreateMap<CreateParecerCommand, ParecerViewModel>().ReverseMap();
            CreateMap<UpdateParecerCommand, ParecerViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}