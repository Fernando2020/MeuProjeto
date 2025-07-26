using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Repositories;

namespace MeuProjeto.Application.UseCases.Users.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;
        private readonly IValidator<LoginRequestDto> _validator;

        public LoginUseCase(IUnitOfWork uow, IUserRepository repo, IValidator<LoginRequestDto> validator)
        {
            _uow = uow;
            _repo = repo;
            _validator = validator;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _repo.GetByEmailAsync(request.Email);
            if (user == null || user.PasswordHash != request.Password)
                throw new UnauthorizedAccessException();

            var access = $"access-{Guid.NewGuid()}";
            var refresh = $"refresh-{Guid.NewGuid()}";
            user.RefreshToken = refresh;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _repo.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new LoginResponseDto { AccessToken = access, RefreshToken = refresh };
        }
    }
}
