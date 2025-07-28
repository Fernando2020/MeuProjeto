using MeuProjeto.Core.Messaging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MeuProjeto.Infrastructure.Messaging
{
    internal class RabbitMqPublisher : IMessagePublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqPublisher(string hostName)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostName
            };
        }

        public async Task PublishAsync<T>(string queueName, T message)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
        }
    }
}
