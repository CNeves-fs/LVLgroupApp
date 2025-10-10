using FluentValidation;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Validators
{
    public class ClaimViewModelValidator : AbstractValidator<ClaimViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public ClaimViewModelValidator()
        {
            //RuleFor(r => r.Claim.DataClaim)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.Claim.MotivoClaim)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.Claim.EmpresaId)
            //    .NotEmpty().WithMessage("Empresa is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.Claim.GrupolojaId)
            //    .NotEmpty().WithMessage("Agrupamento is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.Claim.LojaId)
            //    .NotEmpty().WithMessage("Loja is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.Artigo.ArtigoId)
            //    .NotEmpty().WithMessage("Artigo is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.Artigo.Referencia)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(r => r.Artigo.GenderId)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .GreaterThan(0)
            //    .NotNull();

            //RuleFor(r => r.Artigo.Tamanho)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.Artigo.DataCompra)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.Artigo.DefeitoDoArtigo)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull();

            //RuleFor(r => r.Cliente.Id)
            //    .NotEmpty().WithMessage("Cliente is required.")
            //    .NotNull()
            //    .GreaterThan(0);

            //RuleFor(r => r.Cliente.Nome)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(r => r.Cliente.Telefone)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull()
            //    .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            //RuleFor(r => r.Cliente.NIF)
            //    .MaximumLength(20).WithMessage("{PropertyName} must not exceed 20 characters.");
            ////.MinimumLength(9).WithMessage("{PropertyName} can't exceed 20 characters.");

            //RuleFor(r => r.Cliente.DescriçãoContacto)
            //    .MaximumLength(255).WithMessage("{PropertyName} must not exceed 255 characters.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}