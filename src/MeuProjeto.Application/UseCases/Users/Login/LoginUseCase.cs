using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.Extensions;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Events;
using MeuProjeto.Core.Exceptions;
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
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public LoginUseCase(IUnitOfWork uow, IUserRepository repo, IValidator<LoginRequestDto> validator, IRefreshTokenGenerator refreshTokenGenerator, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, IDomainEventDispatcher domainEventDispatcher)
        {
            _uow = uow;
            _repo = repo;
            _validator = validator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new MyValidationException(validationResult.Errors.ToStringList());

            var user = await _repo.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                throw new MyUnauthorizedException();

            user.RefreshToken = _refreshTokenGenerator.GenerateToken();
            user.RefreshTokenExpiryTime =_refreshTokenGenerator.GetExpirationDate();

            await _repo.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            await _domainEventDispatcher.DispatchAsync(new LoginCompletedEvent(user.Name, user.Email));

            return new LoginResponseDto
            {
                AccessToken = _tokenGenerator.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };
        }
    }
}
