using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task ExecuteAsync(ChangePasswordRequestDto request);
    }
}
