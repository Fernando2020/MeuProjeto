namespace MeuProjeto.Core.Events.LoginCompleted
{
    public class LoginCompletedEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public LoginCompletedEvent(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}