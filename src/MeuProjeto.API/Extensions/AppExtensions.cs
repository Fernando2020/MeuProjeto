using MeuProjeto.Infrastructure.Messaging;

namespace MeuProjeto.Api.Extensions
{
    public static class AppExtensions
    {
        public static async Task InitializeRabbitMqAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var rabbitSetup = scope.ServiceProvider.GetRequiredService<IRabbitMqSetup>();
                await rabbitSetup.DeclareQueuesAndExchangesAsync();
            }
        }
    }
}
