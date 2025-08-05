using MeuProjeto.Core.Constants;
using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Messaging;

namespace MeuProjeto.Core.Events.LoginCompleted
{
    public class SendEmailLoginCompletedHandler : IDomainEventHandler<LoginCompletedEvent>
    {
        private readonly IMessagePublisher _publisher;

        public SendEmailLoginCompletedHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task HandleAsync(LoginCompletedEvent domainEvent)
        {
            var message = new EmailMessage(
                to: domainEvent.Email,
                subject: $"Login realizado, {domainEvent.Name}",
                body: $"Olá {domainEvent.Name}, login realizado com sucesso!"
            );

            await _publisher.PublishAsync(QueueConstants.EMAIL_QUEUE, message);
        }
    }
}