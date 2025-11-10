using AutoMapper;
using Core.Entities.Notifications;
using Core.Features.Notifications.Commands.Create;
using Core.Features.Notifications.Commands.Update;
using Core.Features.Notifications.Response;

namespace Core.Mappings
{
    internal class NotificationProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationProfile()
        {
            CreateMap<CreateNotificationCommand, Notification>().ReverseMap();
            CreateMap<UpdateNotificationCommand, Notification>().ReverseMap();
            CreateMap<NotificationCachedResponse, Notification>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}