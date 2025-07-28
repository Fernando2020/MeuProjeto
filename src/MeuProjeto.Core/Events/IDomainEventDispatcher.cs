namespace MeuProjeto.Core.Events
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent domainEvent);
    }
}
