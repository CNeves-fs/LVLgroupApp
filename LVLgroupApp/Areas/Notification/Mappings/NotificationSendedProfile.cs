using AutoMapper;
using Core.Features.NotificationsSended.Commands.Create;
using Core.Features.NotificationsSended.Commands.Update;
using Core.Features.NotificationsSended.Response;
using LVLgroupApp.Areas.Notification.Models.Notification;

namespace LVLgroupApp.Areas.Notification.Mappings
{
    internal class NotificationSendedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationSendedProfile()
        {
            CreateMap<NotificationSendedCachedResponse, NotificationSendedViewModel>().ReverseMap();
            CreateMap<CreateNotificationSendedCommand, NotificationSendedViewModel>().ReverseMap();
            CreateMap<UpdateNotificationSendedCommand, NotificationSendedViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}