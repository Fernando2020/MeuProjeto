using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.Extensions;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Events;
using MeuProjeto.Core.Exceptions;
using MeuProjeto.Core.Repositories;
using MeuProjeto.Core.Security;

namespace MeuProjeto.Application.UseCases.Users.ChangePassword
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;
        private readonly ILoggedUser _loggedUser;
        private readonly IValidator<ChangePasswordRequestDto> _validator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public ChangePasswordUseCase(IUnitOfWork uow, IUserRepository repo, ILoggedUser loggedUser, IValidator<ChangePasswordRequestDto> validator, IPasswordHasher passwordHasher, IDomainEventDispatcher domainEventDispatcher)
        {
            _uow = uow;
            _repo = repo;
            _loggedUser = loggedUser;
            _validator = validator;
            _passwordHasher = passwordHasher;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task ExecuteAsync(ChangePasswordRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new MyValidationException(validationResult.Errors.ToStringList());

            var loggedUser = await _loggedUser.GetUserAsync();
            
            if(!_passwordHasher.Verify(request.OldPassword, loggedUser.PasswordHash))
                throw new MyDomainException("Senha anterior diferente da senha informada.");

            var user = await _repo.GetByIdAsync(loggedUser.Id);

            user.PasswordHash = _passwordHasher.Hash(request.Password);

            await _repo.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            await _domainEventDispatcher.DispatchAsync(new PasswordChangedEvent(user.Name, user.Email));
        }
    }
}
