using AutoMapper;
using Core.Entities.Claims;
using Core.Features.Statuss.Commands.Create;
using Core.Features.Statuss.Queries.GetAllPaged;
using Core.Features.Statuss.Queries.GetById;
using Core.Features.Statuss.Response;

namespace Core.Mappings
{
    internal class StatusProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public StatusProfile()
        {
            CreateMap<CreateStatusCommand, Status>().ReverseMap();
            CreateMap<StatusCachedResponse, Status>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}