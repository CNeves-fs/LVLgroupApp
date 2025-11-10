using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.Prazolimite;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class PrazolimiteViewModelValidator : AbstractValidator<PrazolimiteViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public PrazolimiteViewModelValidator()
        {
            RuleFor(p => p.Alarme)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.LimiteMin)
                .LessThanOrEqualTo(30)
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.LimiteMax)
                .LessThanOrEqualTo(30)
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.Cortexto)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Corfundo)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}