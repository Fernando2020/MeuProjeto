namespace MeuProjeto.Core.Events
{
    public class UserRegisteredEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }

        public UserRegisteredEvent(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}