using MeuProjeto.Worker.Consumers;

namespace MeuProjeto.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyHostedService(this IServiceCollection services)
        {
            services.AddHostedService<UserRegisteredConsumer>();
            services.AddHostedService<PasswordChangedConsumer>();

            return services;
        }
    }
}
