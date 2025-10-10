using FluentValidation;
using LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia;

namespace LVLgroupApp.Areas.Artigo.Validators
{
    public class TipoOcorrenciaViewModelValidator : AbstractValidator<TipoOcorrenciaViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaViewModelValidator()
        {
            RuleFor(to => to.DefaultName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(to => to.EsName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 128 characters.");

            RuleFor(to => to.EnName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 128 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}