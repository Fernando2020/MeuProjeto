using FluentValidation;
using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.RefreshToken
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshTokenRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken é obrigatório");
        }
    }
}
