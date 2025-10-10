using FluentValidation;
using LVLgroupApp.Areas.Notification.Models.Notification;

namespace LVLgroupApp.Areas.Notification.Validators
{
    public class NotificationViewModelValidator : AbstractValidator<NotificationViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public NotificationViewModelValidator()
        {
            RuleFor(n => n.Subject)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(n => n.Text)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }


        //---------------------------------------------------------------------------------------------------

    }
}