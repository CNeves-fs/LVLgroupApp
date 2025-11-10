using AutoMapper;
using Core.Entities.Notifications;
using Core.Features.NotificationsSended.Commands.Create;
using Core.Features.NotificationsSended.Commands.Update;
using Core.Features.NotificationsSended.Response;

namespace Core.Mappings
{
    internal class NotificationSendedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationSendedProfile()
        {
            CreateMap<CreateNotificationSendedCommand, NotificationSended>().ReverseMap();
            CreateMap<UpdateNotificationSendedCommand, NotificationSended>().ReverseMap();
            CreateMap<NotificationSendedCachedResponse, NotificationSended>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}