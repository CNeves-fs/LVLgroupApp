using AutoMapper;
using Core.Entities.Claims;
using Core.Features.Fotos.Commands.Create;
using Core.Features.Fotos.Commands.Update;
using Core.Features.Fotos.Queries.GetAllPaged;
using Core.Features.Fotos.Queries.GetById;
using Core.Features.Fotos.Response;

namespace Core.Mappings
{
    internal class FotoEntitieProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public FotoEntitieProfile()
        {
            CreateMap<CreateFotoCommand, Foto>().ForMember(d => d.TempFolderGuid, d => d.MapFrom(x => x.ClaimFolder)).ReverseMap();
            CreateMap<UpdateFotoCommand, Foto>().ForMember(d => d.TempFolderGuid, d => d.MapFrom(x => x.ClaimFolder)).ReverseMap();
            CreateMap<FotoCachedResponse, Foto>().ForMember(d => d.TempFolderGuid, d => d.MapFrom(x => x.ClaimFolder)).ReverseMap();

        }


        //---------------------------------------------------------------------------------------------------

    }
}