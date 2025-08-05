using MeuProjeto.Core.Constants;
using MeuProjeto.Core.Events.Messages;
using MeuProjeto.Core.Services;
using MeuProjeto.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MeuProjeto.Worker.Consumers
{
    public class EmailConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly ILogger<EmailConsumer> _logger;

        private const string QueueName = QueueConstants.EMAIL_QUEUE;

        public EmailConsumer(
            IServiceProvider serviceProvider,
            IRabbitMqConnection rabbitMqConnection,
            ILogger<EmailConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _rabbitMqConnection = rabbitMqConnection;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = await _rabbitMqConnection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<EmailMessage>(json);

                    if (message is not null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        await emailService.SendEmailAsync(message.To, message.Subject, message.Body);
                    }

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem da fila {Queue}", QueueName);
                    // Possível implementação de retry ou DLQ
                }
            };

            await channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );
        }
    }
}
