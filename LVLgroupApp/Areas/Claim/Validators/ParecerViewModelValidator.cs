using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.ParecerTécnico;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class ParecerViewModelValidator : AbstractValidator<ParecerViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public ParecerViewModelValidator()
        {
            //RuleFor(p => p.Opinião)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();
        }


        //---------------------------------------------------------------------------------------------------

    }
}