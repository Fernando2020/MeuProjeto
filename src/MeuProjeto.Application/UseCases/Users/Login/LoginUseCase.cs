using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Exceptions;
using MeuProjeto.Core.Extensions;
using MeuProjeto.Core.Repositories;
using MeuProjeto.Core.Security;

namespace MeuProjeto.Application.UseCases.Users.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;
        private readonly IValidator<LoginRequestDto> _validator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginUseCase(IUnitOfWork uow, IUserRepository repo, IValidator<LoginRequestDto> validator, IRefreshTokenGenerator refreshTokenGenerator, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _uow = uow;
            _repo = repo;
            _validator = validator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (validationResult.IsValid.IsFalse())
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();
                throw new MyValidationException(errors);
            }

            var user = await _repo.GetByEmailAsync(request.Email);
            if (user == null || _passwordHasher.Verify(request.Password, user.PasswordHash).IsFalse())
                throw new MyUnauthorizedException();

            user.RefreshToken = _refreshTokenGenerator.GenerateToken();
            user.RefreshTokenExpiryTime =_refreshTokenGenerator.GetExpirationDate();

            await _repo.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = _tokenGenerator.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };
        }
    }
}
