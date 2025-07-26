using FluentValidation;
using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.Login
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Formato de email inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter ao menos 6 caracteres");
        }
    }
}
