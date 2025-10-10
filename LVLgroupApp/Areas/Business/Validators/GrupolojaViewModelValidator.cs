using FluentValidation;
using LVLgroupApp.Areas.Business.Models.Loja;

namespace LVLgroupApp.Areas.Business.Validators
{
    public class GrupolojaViewModelValidator : AbstractValidator<LojaViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public GrupolojaViewModelValidator()
        {
            RuleFor(l => l.Nome)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}