namespace MeuProjeto.Core.Events
{
    public interface IDomainEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent domainEvent);
    }
}
