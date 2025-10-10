using FluentValidation;
using LVLgroupApp.Areas.Cliente.Models.Cliente;

namespace LVLgroupApp.Areas.Business.Validators
{
    public class ClienteViewModelValidator : AbstractValidator<ClienteViewModel>
    {

        //---------------------------------------------------------------------------------------------------


        public ClienteViewModelValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(e => e.Telefone)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(e => e.NIF)
                .MaximumLength(20).WithMessage("{PropertyName} must not exceed 20 characters.");
                //.MinimumLength(9).WithMessage("{PropertyName} can't exceed 20 characters.");

            RuleFor(e => e.DescriçãoContacto)
                .MaximumLength(255).WithMessage("{PropertyName} must not exceed 255 characters.");

        }


        //---------------------------------------------------------------------------------------------------

    }
}