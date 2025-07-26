using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.UseCases.Users.Login;
using MeuProjeto.Application.UseCases.Users.RefreshToken;
using Microsoft.Extensions.DependencyInjection;

namespace MeuProjeto.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            AddUseCases(services);
            AddValidators(services);

            return services;
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        }

        private static void AddValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
            services.AddScoped<IValidator<RefreshTokenRequestDto>, RefreshTokenRequestDtoValidator>();
        }
    }
}