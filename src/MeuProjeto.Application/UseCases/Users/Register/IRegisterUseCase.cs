using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.Register
{
    public interface IRegisterUseCase
    {
        Task<RegisterResponseDto> ExecuteAsync(RegisterRequestDto request);
    }
}
