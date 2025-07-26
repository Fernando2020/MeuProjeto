using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.Login
{
    public interface ILoginUseCase
    {
        Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request);
    }
}
