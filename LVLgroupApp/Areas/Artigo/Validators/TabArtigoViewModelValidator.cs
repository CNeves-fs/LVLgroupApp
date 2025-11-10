using FluentValidation;
using LVLgroupApp.Areas.Artigo.Models.Artigo;

namespace LVLgroupApp.Areas.Artigo.Validators
{
    public class TabArtigoViewModelValidator : AbstractValidator<TabArtigoViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public TabArtigoViewModelValidator()
        {
            //RuleFor(a => a.ArtigoId)
            //    .NotEmpty().WithMessage("Artigo is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(a => a.Referencia)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(a => a.GenderId)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(a => a.Tamanho)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(a => a.DataCompra)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(a => a.DefeitoDoArtigo)
            //.NotEmpty().WithMessage("{PropertyName} is required.")
            //.NotNull();

        }


        //---------------------------------------------------------------------------------------------------

    }
}