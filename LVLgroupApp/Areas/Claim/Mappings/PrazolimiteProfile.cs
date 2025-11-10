using AutoMapper;
using Core.Features.Prazoslimite.Commands.Create;
using Core.Features.Prazoslimite.Commands.Update;
using Core.Features.Prazoslimite.Queries.GetAllCached;
using Core.Features.Prazoslimite.Queries.GetById;
using Core.Features.Prazoslimite.Response;
using LVLgroupApp.Areas.Claim.Models.Prazolimite;

namespace LVLgroupApp.Areas.Claim.Mappings
{
    internal class PrazolimiteProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public PrazolimiteProfile()
        {
            CreateMap<PrazolimiteCachedResponse, PrazolimiteViewModel>().ReverseMap();
            CreateMap<CreatePrazolimiteCommand, PrazolimiteViewModel>().ReverseMap();
            CreateMap<UpdatePrazolimiteCommand, PrazolimiteViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}