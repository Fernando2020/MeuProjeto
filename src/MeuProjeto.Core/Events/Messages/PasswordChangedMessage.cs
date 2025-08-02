namespace MeuProjeto.Core.Events.Messages
{
    public class PasswordChangedMessage
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
