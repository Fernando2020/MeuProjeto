using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Exceptions;
using MeuProjeto.Core.Repositories;

namespace MeuProjeto.Application.UseCases.Users.RefreshToken
{
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;
        private readonly IValidator<RefreshTokenRequestDto> _validator;

        public RefreshTokenUseCase(IUnitOfWork uow, IUserRepository repo, IValidator<RefreshTokenRequestDto> validator)
        {
            _uow = uow;
            _repo = repo;
            _validator = validator;
        }

        public async Task<LoginResponseDto> ExecuteAsync(RefreshTokenRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();
                throw new MyValidationException(errors);
            }

            var user = await _repo.GetByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new MyUnauthorizedException();

            var newAccess = $"access-{Guid.NewGuid()}";
            var newRefresh = $"refresh-{Guid.NewGuid()}";
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _repo.UpdateAsync(user);
            await _uow.SaveChangesAsync();

            return new LoginResponseDto { AccessToken = newAccess, RefreshToken = newRefresh };
        }
    }
}
