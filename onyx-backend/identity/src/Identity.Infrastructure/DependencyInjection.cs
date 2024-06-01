using Identity.Application.Abstractions.Authentication;
using Identity.Domain;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Email;
using Identity.Infrastructure.Messanger;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedDAL;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddMessanger(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<MessangerOptionsSetup>();
        services.AddSingleton<MessangerClient>();
        services.AddScoped<IEmailService, EmailService>();
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<AuthenticationOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddScoped<IJwtService, JwtService>();
    }
}