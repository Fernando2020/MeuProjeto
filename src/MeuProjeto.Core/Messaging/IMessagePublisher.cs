namespace MeuProjeto.Core.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string queueName, T message);
    }
}
