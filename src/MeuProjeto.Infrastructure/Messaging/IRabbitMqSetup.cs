namespace MeuProjeto.Infrastructure.Messaging
{
    public interface IRabbitMqSetup
    {
        Task DeclareQueuesAndExchangesAsync();
    }
}
