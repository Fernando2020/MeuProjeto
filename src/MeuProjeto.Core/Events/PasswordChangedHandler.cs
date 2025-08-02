using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Messaging;

namespace MeuProjeto.Core.Events
{
    public class PasswordChangedHandler : IDomainEventHandler<PasswordChangedEvent>
    {
        private readonly IMessagePublisher _publisher;

        public PasswordChangedHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task HandleAsync(PasswordChangedEvent domainEvent)
        {
            var message = new PasswordChangedMessage
            {
                Name = domainEvent.Name,
                Email = domainEvent.Email
            };

            await _publisher.PublishAsync("email:changed-password", message);
        }
    }
}