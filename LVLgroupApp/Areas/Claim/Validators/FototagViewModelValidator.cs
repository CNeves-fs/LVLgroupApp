using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.Fototag;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class FototagViewModelValidator : AbstractValidator<FototagViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public FototagViewModelValidator()
        {

            RuleFor(f => f.Tag)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}