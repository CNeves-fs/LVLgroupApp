using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.ParecerTécnico;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class TabPareceresViewModelValidator : AbstractValidator<TabPareceresViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public TabPareceresViewModelValidator()
        {
            //RuleFor(p => p.ParecerColaborador.Opinião)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();
        }


        //---------------------------------------------------------------------------------------------------

    }
}