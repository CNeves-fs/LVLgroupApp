using FluentValidation;
using LVLgroupApp.Areas.Business.Models.Mercado;

namespace LVLgroupApp.Areas.Business.Validators
{
    public class MercadoViewModelValidator : AbstractValidator<MercadoViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public MercadoViewModelValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(e => e.NomeCurto)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(3).WithMessage("{PropertyName} must not exceed 3 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}