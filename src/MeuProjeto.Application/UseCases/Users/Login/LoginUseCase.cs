using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Core.Data;
using MeuProjeto.Core.Repositories;

namespace MeuProjeto.Application.UseCases.Users.Login
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserRepository _repo;

        public LoginUseCase(IUnitOfWork uow, IUserRepository repo)
        {
            _uow = uow;
            _repo = repo;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request)
        {
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
