using MeuProjeto.Core.Data;
using MeuProjeto.Core.Events;
using MeuProjeto.Core.Events.LoginCompleted;
using MeuProjeto.Core.Events.PasswordChanged;
using MeuProjeto.Core.Events.UserRegistered;
using MeuProjeto.Core.Messaging;
using MeuProjeto.Core.Repositories;
using MeuProjeto.Core.Security;
using MeuProjeto.Core.Services;
using MeuProjeto.Infrastructure.Data;
using MeuProjeto.Infrastructure.Events;
using MeuProjeto.Infrastructure.Messaging;
using MeuProjeto.Infrastructure.Repositories;
using MeuProjeto.Infrastructure.Security;
using MeuProjeto.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuProjeto.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddSecurity(services, configuration);
            AddLoggedUser(services);
            AddEvents(services);
            AddRabbitMq(services, configuration);
            AddServices(services);

            return services;
        }

        public static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        public static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddSecurity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddJwtAuthentication(configuration);
        }

        public static void AddLoggedUser(IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }

        public static void AddEvents(IServiceCollection services)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IDomainEventHandler<UserRegisteredEvent>, SendEmailUserRegisteredHandler>();
            services.AddScoped<IDomainEventHandler<PasswordChangedEvent>, SendEmailPasswordChangedHandler>();
            services.AddScoped<IDomainEventHandler<LoginCompletedEvent>, SendEmailLoginCompletedHandler>();
        }

        public static void AddRabbitMq(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
        }

        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, ConsoleEmailService>();
        }
    }
}
