using FluentValidation;
using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;

namespace LVLgroupApp.Areas.Vendas.Validators
{
    public class VendaSemanalViewModelValidator : AbstractValidator<VendaSemanalViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public VendaSemanalViewModelValidator()
        {
            RuleFor(v => v.LojaId)
                .GreaterThan(0).WithMessage("{PropertyName} is required.");
            RuleFor(v => v.GrupolojaId)
                .GreaterThan(0).WithMessage("{PropertyName} is required.");
            RuleFor(v => v.EmpresaId)
                .GreaterThan(0).WithMessage("{PropertyName} is required.");
            RuleFor(v => v.MercadoId)
                .GreaterThan(0).WithMessage("{PropertyName} is required.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}