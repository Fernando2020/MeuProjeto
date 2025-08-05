using MeuProjeto.Core.Constants;
using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Messaging;

namespace MeuProjeto.Core.Events.PasswordChanged
{
    public class SendEmailPasswordChangedHandler : IDomainEventHandler<PasswordChangedEvent>
    {
        private readonly IMessagePublisher _publisher;

        public SendEmailPasswordChangedHandler(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task HandleAsync(PasswordChangedEvent domainEvent)
        {
            var message = new EmailMessage(
                to: domainEvent.Email,
                subject: $"Alteração de senha, {domainEvent.Name}",
                body: $"Olá {domainEvent.Name}, senha alterada com sucesso!"
            );

            await _publisher.PublishAsync(QueueConstants.EMAIL_QUEUE, message);
        }
    }
}