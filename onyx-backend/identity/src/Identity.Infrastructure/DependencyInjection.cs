using Identity.Application.Abstractions.Authentication;
using Identity.Domain;
using Identity.Infrastructure.Authentication;
using Identity.Infrastructure.Email;
using Identity.Infrastructure.Messanger;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using SharedDAL;
using SharedDAL.DataSettings;

namespace Identity.Infrastructure;

public static class DependencyInjection
{
    public static void InjectInfrastructure(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddPersistence(builder.GetContext().Configuration);
        builder.Services.AddMessanger(builder.GetContext().Configuration);
        builder.Services.AddAuthentication(builder.GetContext().Configuration);
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<CosmosDbOptionsSetup>();
        services.AddScoped<CosmosDbContext>();
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

        //services
        //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer();

        services
            .AddFunctionAuthentication(
                options =>
                {
                    var jwtSchema = "CustomJwtBearerSchema";
                    options.DefaultScheme = jwtSchema;
                    options.DefaultSignInScheme = jwtSchema;
                    options.DefaultChallengeScheme = jwtSchema;
                    options.DefaultAuthenticateScheme = jwtSchema;
                });

        services.AddFunctionAuthorization(options => { });

        services.AddScoped<IJwtService, JwtService>();
    }
}