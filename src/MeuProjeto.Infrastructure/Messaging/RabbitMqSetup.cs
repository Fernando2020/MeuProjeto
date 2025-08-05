using MeuProjeto.Core.Constants;
using RabbitMQ.Client;

namespace MeuProjeto.Infrastructure.Messaging
{
    public class RabbitMqSetup : IRabbitMqSetup
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqSetup(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task DeclareQueuesAndExchangesAsync()
        {
            var channel = await _connection.CreateChannelAsync();

            // Fila principal
            await channel.QueueDeclareAsync(
                queue: QueueConstants.EMAIL_QUEUE,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    { "x-dead-letter-exchange", "" }, // Exchange padrão (default)
                    { "x-dead-letter-routing-key", QueueConstants.EMAIL_QUEUE_DLQ }
                }
            );

            // DLQ
            await channel.QueueDeclareAsync(
                queue: QueueConstants.EMAIL_QUEUE_DLQ,
                durable: true,
                exclusive: false,
                autoDelete: false
            );


            await channel.CloseAsync();
        }
    }
}