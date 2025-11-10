using AutoMapper;
using Core.Features.Notifications.Commands.Create;
using Core.Features.Notifications.Commands.Update;
using Core.Features.Notifications.Response;
using LVLgroupApp.Areas.Notification.Models.Notification;

namespace LVLgroupApp.Areas.Notification.Mappings
{
    internal class NotificationProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationProfile()
        {
            CreateMap<NotificationCachedResponse, NotificationViewModel>().ReverseMap();
            CreateMap<CreateNotificationCommand, NotificationViewModel>().ReverseMap();
            CreateMap<UpdateNotificationCommand, NotificationViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}