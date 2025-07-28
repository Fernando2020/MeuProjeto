
namespace MeuProjeto.Core.Events
{
    public class SendWelcomeEmailHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        public Task HandleAsync(UserRegisteredEvent domainEvent)
        {
            Console.WriteLine($"[Email] Bem-vindo, {domainEvent.Name} ({domainEvent.Email})");

            return Task.CompletedTask;
        }
    }
}
