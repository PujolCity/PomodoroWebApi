using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PomodoroWebApp.Application.Validators;

namespace PomodoroWebApp.Application.AppModule;

public static class UserModule
{
    public static IServiceCollection AddUserValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterRequestDTOValidator>();
        return services;
    }
}