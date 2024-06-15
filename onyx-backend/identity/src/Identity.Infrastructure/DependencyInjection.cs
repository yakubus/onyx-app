using Identity.Application.Abstractions.Authentication;
using Identity.Domain;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Data.Services;
using Identity.Infrastructure.Email;
using Identity.Infrastructure.Messanger;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static void InjectInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence();
        services.AddAuthentication();
        services.AddContexts();
        services.AddMessanger();
    }

    private static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<DbContext>();
        services.AddScoped(typeof(IDataModelService<>), typeof(DataModelService<>));
        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddMessanger(this IServiceCollection services)
    {
        services.ConfigureOptions<MessangerOptionsSetup>();
        services.AddSingleton<MessangerClient>();
        services.AddScoped<IEmailService, EmailService>();
    }

    private static void AddContexts(this IServiceCollection services)
    {
        //TODO consider
        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
    }

    private static void AddAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<AuthenticationOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddScoped<IJwtService, JwtService>();
    }
}