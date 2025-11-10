using FluentValidation;
using LVLgroupApp.Areas.Artigo.Models.Gender;

namespace LVLgroupApp.Areas.Artigo.Validators
{
    public class GenderViewModelValidator : AbstractValidator<GenderViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public GenderViewModelValidator()
        {
            //RuleFor(g => g.Nome)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(g => g.TamanhosNum)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();
        }


        //---------------------------------------------------------------------------------------------------

    }
}