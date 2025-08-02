using MeuProjeto.Core.Events.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MeuProjeto.Worker.Consumers
{
    public class LoginCompletedConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public LoginCompletedConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.QueueDeclareAsync(queue: "email:login-completed", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var message = JsonSerializer.Deserialize<LoginCompletedMessage>(json);

                if (message != null)
                {
                    using var scope = _serviceProvider.CreateAsyncScope();

                    Console.WriteLine($"[Email] Login realizado, {message.Name} ({message.Email})");
                }



                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            };

            await channel.BasicConsumeAsync(queue: "email:login-completed", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        }
    }
}