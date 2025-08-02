namespace MeuProjeto.Core.Events
{
    public class PasswordChangedEvent
    {
        public string Name { get; }
        public string Email { get; }

        public PasswordChangedEvent(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}