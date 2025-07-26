using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.RefreshToken
{
    public interface IRefreshTokenUseCase
    {
        Task<LoginResponseDto> ExecuteAsync(RefreshTokenRequestDto request);
    }
}
