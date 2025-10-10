using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class TabClaimViewModelValidator : AbstractValidator<TabClaimViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public TabClaimViewModelValidator()
        {
            //RuleFor(r => r.DataClaim)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.MotivoClaim)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.EmpresaId)
            //    .NotEmpty().WithMessage("Empresa is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.GrupolojaId)
            //    .NotEmpty().WithMessage("Agrupamento is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.LojaId)
            //    .NotEmpty().WithMessage("Loja is required.")
            //    .GreaterThan(0)
            //    .NotNull();
        }


        //---------------------------------------------------------------------------------------------------

    }
}