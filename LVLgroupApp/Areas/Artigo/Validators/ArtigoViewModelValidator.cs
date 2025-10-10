using FluentValidation;
using LVLgroupApp.Areas.Artigo.Models.Artigo;

namespace LVLgroupApp.Areas.Artigo.Validators
{
    public class ArtigoViewModelValidator : AbstractValidator<ArtigoViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public ArtigoViewModelValidator()
        {
            //RuleFor(a => a.Referencia)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(a => a.EmpresaId)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(a => a.GenderId)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(a => a.Cor)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

        }


        //---------------------------------------------------------------------------------------------------

    }
}