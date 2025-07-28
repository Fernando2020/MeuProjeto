using MeuProjeto.Core.Events;
using Microsoft.Extensions.DependencyInjection;

namespace MeuProjeto.Infrastructure.Events
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<TEvent>(TEvent domainEvent)
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<TEvent>>();
            var exceptions = new List<Exception>();

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.HandleAsync(domainEvent);
                }
                catch (Exception ex)
                {
                    exceptions.Add(new Exception($"Erro no handler: {handler.GetType().Name}", ex));
                }
            }

            if (exceptions.Any())
                throw new AggregateException("Um ou mais handlers falharam.", exceptions);
        }
    }
}
