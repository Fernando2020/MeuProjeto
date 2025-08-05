using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace MeuProjeto.Infrastructure.Messaging
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IOptions<RabbitMqSettings> _settings;
        private readonly ILogger<RabbitMqConnection> _logger;
        private IConnection? _connection;

        public bool IsConnected => _connection?.IsOpen == true;

        public RabbitMqConnection(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqConnection> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        private readonly SemaphoreSlim _connectionLock = new(1, 1);

        public async Task TryConnectAsync()
        {
            if (IsConnected)
                return;

            await _connectionLock.WaitAsync();
            try
            {
                if (IsConnected) return;

                var factory = new ConnectionFactory
                {
                    HostName = _settings.Value.Host,
                    UserName = _settings.Value.Username,
                    Password = _settings.Value.Password,
                    VirtualHost = _settings.Value.VirtualHost,
                    Port = _settings.Value.Port
                };

                _connection = await factory.CreateConnectionAsync();
                _logger.LogInformation("Conectado ao RabbitMQ com sucesso.");
            }
            catch (BrokerUnreachableException ex) when (ex.InnerException is SocketException)
            {
                _logger.LogError(ex, "Erro ao conectar ao RabbitMQ.");
                throw;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async ValueTask<IChannel> CreateChannelAsync()
        {
            if (!IsConnected)
                await TryConnectAsync();

            return await _connection!.CreateChannelAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is { IsOpen: true })
                await _connection.DisposeAsync();
        }
    }
}
