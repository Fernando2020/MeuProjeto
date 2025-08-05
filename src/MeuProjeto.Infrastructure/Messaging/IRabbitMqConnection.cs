using RabbitMQ.Client;

namespace MeuProjeto.Infrastructure.Messaging
{
    public interface IRabbitMqConnection : IAsyncDisposable
    {
        bool IsConnected { get; }
        ValueTask<IChannel> CreateChannelAsync();
        Task TryConnectAsync();
    }
}
