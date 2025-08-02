using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Messaging;

namespace MeuProjeto.Core.Events
{
    public class LoginCompletedHandler : IDomainEventHandler<LoginCompletedEvent>
    {
        private readonly IMessagePublisher _publisher;

        public LoginCompletedHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task HandleAsync(LoginCompletedEvent domainEvent)
        {
            var message = new LoginCompletedMessage
            {
                Name = domainEvent.Name,
                Email = domainEvent.Email
            };

            await _publisher.PublishAsync("email:login-completed", message);
        }
    }
}