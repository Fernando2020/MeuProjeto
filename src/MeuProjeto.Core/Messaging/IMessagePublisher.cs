namespace MeuProjeto.Core.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string queueName, object message);
    }
}
