namespace MeuProjeto.Application.DTOs.Users
{
    public class RegisterResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
