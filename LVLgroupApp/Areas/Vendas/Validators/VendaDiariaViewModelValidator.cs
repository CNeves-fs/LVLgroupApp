using FluentValidation;
using LVLgroupApp.Areas.Vendas.Models.VendaDiaria;

namespace LVLgroupApp.Areas.Vendas.Validators
{
    public class VendaDiariaViewModelValidator : AbstractValidator<VendaDiariaViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public VendaDiariaViewModelValidator()
        {
            RuleFor(v => v.ValorDaVenda)
                .GreaterThan(0).WithMessage("{PropertyName} is required.");
        }


        //---------------------------------------------------------------------------------------------------

    }
}