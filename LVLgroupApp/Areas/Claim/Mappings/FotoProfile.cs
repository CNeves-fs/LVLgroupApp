using AutoMapper;
using Core.Features.Fotos.Commands.Create;
using Core.Features.Fotos.Commands.Update;
using Core.Features.Fotos.Response;
using LVLgroupApp.Areas.Claim.Models.Foto;

namespace LVLgroupApp.Areas.Claim.Mappings
{
    internal class FotoProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public FotoProfile()
        {
            CreateMap<FotoCachedResponse, FotoViewModel>().ReverseMap();
            CreateMap<CreateFotoCommand, FotoViewModel>().ReverseMap();
            CreateMap<UpdateFotoCommand, FotoViewModel>().ReverseMap();
            CreateMap<FotoUploaderViewModel, FotoViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}