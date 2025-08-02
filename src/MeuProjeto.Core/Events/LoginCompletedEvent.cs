namespace MeuProjeto.Core.Events
{
    public class LoginCompletedEvent
    {
        public string Name { get; }
        public string Email { get; }

        public LoginCompletedEvent(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
