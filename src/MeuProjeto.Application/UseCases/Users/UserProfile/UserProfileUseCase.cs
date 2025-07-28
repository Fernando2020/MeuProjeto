using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Core.Security;

namespace MeuProjeto.Application.UseCases.Users.UserProfile
{
    public class UserProfileUseCase : IUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;

        public UserProfileUseCase(ILoggedUser loggedUser)
        {
            _loggedUser = loggedUser;
        }

        public async Task<UserProfileResponseDto> ExecuteAsync()
        {
            var user = await _loggedUser.GetUserAsync();

            return new UserProfileResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
