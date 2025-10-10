using FluentValidation;
using LVLgroupApp.Areas.Business.Models.Loja;

namespace LVLgroupApp.Areas.Business.Validators
{
    public class LojaViewModelValidator : AbstractValidator<LojaViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public LojaViewModelValidator()
        {
            RuleFor(l => l.Nome)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(l => l.NomeCurto)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MinimumLength(4).WithMessage("{PropertyName} must have 4 characters.")
                .MaximumLength(4).WithMessage("{PropertyName} must not exceed 4 characters.");

            RuleFor(l => l.Cidade)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(l => l.País)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}