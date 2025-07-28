using MeuProjeto.Application.DTOs.Users;

namespace MeuProjeto.Application.UseCases.Users.UserProfile
{
    public interface IUserProfileUseCase
    {
        Task<UserProfileResponseDto> ExecuteAsync();
    }
}
