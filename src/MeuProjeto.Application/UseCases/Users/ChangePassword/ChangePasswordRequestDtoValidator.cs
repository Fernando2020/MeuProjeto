using FluentValidation;
using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.ChangePassword
{
    public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Senha anterior é obrigatória");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter ao menos 6 caracteres");

            RuleFor(x => x.ConfirmationPassword)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter ao menos 6 caracteres")
                .Equal(x => x.Password).WithMessage("Confirmação de Senha deve ser igual a Senha");
        }
    }
}
