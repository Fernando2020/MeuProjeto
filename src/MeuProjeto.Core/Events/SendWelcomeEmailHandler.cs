
using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Messaging;

namespace MeuProjeto.Core.Events
{
    public class SendWelcomeEmailHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        private readonly IMessagePublisher _publisher;

        public SendWelcomeEmailHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task HandleAsync(UserRegisteredEvent domainEvent)
        {
            var message = new SendWelcomeEmailMessage
            {
                Name = domainEvent.Name,
                Email = domainEvent.Email
            };

            await _publisher.PublishAsync("email:welcome", message);
        }
    }
}
