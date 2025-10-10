using AutoMapper;
using Core.Entities.Claims;
using Core.Features.Statuss.Commands.Create;
using Core.Features.Statuss.Commands.Update;
using Core.Features.Statuss.Queries.GetAllCached;
using Core.Features.Statuss.Queries.GetById;
using Core.Features.Statuss.Response;
using LVLgroupApp.Areas.Claim.Models.Status;

namespace LVLgroupApp.Areas.Claim.Mappings
{
    internal class StatusProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public StatusProfile()
        {
            CreateMap<Status, StatusViewModel>().ReverseMap();
            CreateMap<StatusCachedResponse, StatusViewModel>().ReverseMap();
            CreateMap<CreateStatusCommand, StatusViewModel>().ReverseMap();
            CreateMap<UpdateStatusCommand, StatusViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}