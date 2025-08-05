namespace MeuProjeto.Core.Events.PasswordChanged
{
    public class PasswordChangedEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public PasswordChangedEvent(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}