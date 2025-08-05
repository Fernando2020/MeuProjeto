using MeuProjeto.Core.Messaging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MeuProjeto.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqPublisher(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishAsync(string queueName, object message)
        {
            var channel = await _connection.CreateChannelAsync();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queueName,
                mandatory: false,
                body: body
            );
        }
    }
}
