using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.Status;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class StatusViewModelValidator : AbstractValidator<StatusViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public StatusViewModelValidator()
        {
            RuleFor(s => s.Texto)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(s => s.Cortexto)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(s => s.Corfundo)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}