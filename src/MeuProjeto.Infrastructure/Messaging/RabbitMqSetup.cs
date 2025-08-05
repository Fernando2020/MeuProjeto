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

            // Exchange principal vazia porque você está usando fila direta sem exchange explícita
            // Se quiser usar exchange, declare aqui

            // Declare fila principal com dead letter configurado
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

            // Declare fila DLQ
            await channel.QueueDeclareAsync(
                queue: QueueConstants.EMAIL_QUEUE_DLQ,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            // Se quiser usar Retry com TTL depois, declare a fila retry aqui

            // Opcional: outros bindings e exchanges, caso use

            await channel.CloseAsync();
        }
    }
}