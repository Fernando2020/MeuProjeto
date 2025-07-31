using FluentValidation;
using MeuProjeto.Application.DTOs.Users;
using MeuProjeto.Application.UseCases.Users.ChangePassword;
using MeuProjeto.Application.UseCases.Users.Login;
using MeuProjeto.Application.UseCases.Users.RefreshToken;
using MeuProjeto.Application.UseCases.Users.Register;
using MeuProjeto.Application.UseCases.Users.UserProfile;
using Microsoft.Extensions.DependencyInjection;

namespace MeuProjeto.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyApplication(this IServiceCollection services)
        {
            AddUseCases(services);
            AddValidators(services);

            return services;
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<ILoginUseCase, LoginUseCase>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<IRegisterUseCase, RegisterUseCase>();
            services.AddScoped<IUserProfileUseCase, UserProfileUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        }

        private static void AddValidators(IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
            services.AddScoped<IValidator<RefreshTokenRequestDto>, RefreshTokenRequestDtoValidator>();
            services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
            services.AddScoped<IValidator<ChangePasswordRequestDto>, ChangePasswordRequestDtoValidator>();
        }
    }
}