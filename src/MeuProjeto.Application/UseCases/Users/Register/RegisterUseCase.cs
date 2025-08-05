using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.Extensions;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Entities;
using MeuProjeto.Core.Events;
using MeuProjeto.Core.Events.UserRegistered;
using MeuProjeto.Core.Exceptions;
using MeuProjeto.Core.Repositories;
using MeuProjeto.Core.Security;

namespace MeuProjeto.Application.UseCases.Users.Register
{
    public class RegisterUseCase : IRegisterUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;
        private readonly IValidator<RegisterRequestDto> _validator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public RegisterUseCase(IUnitOfWork uow, IUserRepository repo, IValidator<RegisterRequestDto> validator, IRefreshTokenGenerator refreshTokenGenerator, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, IDomainEventDispatcher domainEventDispatcher)
        {
            _uow = uow;
            _repo = repo;
            _validator = validator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<RegisterResponseDto> ExecuteAsync(RegisterRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new MyValidationException(validationResult.Errors.ToStringList());

            var exists = await _repo.GetByEmailAsync(request.Email);
            if (exists is not null)
                throw new MyDomainException("E-mail já cadastrado.");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email
            };

            user.PasswordHash = _passwordHasher.Hash(request.Password);
            user.RefreshToken = _refreshTokenGenerator.GenerateToken();
            user.RefreshTokenExpiryTime = _refreshTokenGenerator.GetExpirationDate();

            await _repo.AddAsync(user);
            await _uow.SaveChangesAsync();

            await _domainEventDispatcher.DispatchAsync(new UserRegisteredEvent(user.Id, user.Name, user.Email));

            return new RegisterResponseDto
            {
                AccessToken = _tokenGenerator.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };
        }
    }
}
