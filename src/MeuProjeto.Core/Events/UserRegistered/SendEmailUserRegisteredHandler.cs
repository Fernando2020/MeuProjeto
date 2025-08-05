using MeuProjeto.Core.Constants;
using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Messaging;

namespace MeuProjeto.Core.Events.UserRegistered
{
    public class SendEmailUserRegisteredHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        private readonly IMessagePublisher _publisher;

        public SendEmailUserRegisteredHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task HandleAsync(UserRegisteredEvent domainEvent)
        {
            var message = new EmailMessage(
                to: domainEvent.Email,
                subject: $"Bem-vindo, {domainEvent.Name}",
                body: $"Olá {domainEvent.Name}, seja bem-vindo à nossa plataforma!"
            );

            await _publisher.PublishAsync(QueueConstants.EMAIL_QUEUE, message);
        }
    }
}
