using FluentValidation;
using LVLgroupApp.Areas.Notification.Models.Notification;

namespace LVLgroupApp.Areas.Notification.Validators
{
    public class NotificationSendedViewModelValidator : AbstractValidator<NotificationSendedViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationSendedViewModelValidator()
        {
            RuleFor(n => n.NotificationId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(n => n.ToUserId)
                 .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }


        //---------------------------------------------------------------------------------------------------

    }
}