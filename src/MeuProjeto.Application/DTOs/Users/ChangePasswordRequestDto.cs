namespace MeuProjeto.Application.DTOs.Users
{
    public class ChangePasswordRequestDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmationPassword { get; set; } = string.Empty;
    }
}
