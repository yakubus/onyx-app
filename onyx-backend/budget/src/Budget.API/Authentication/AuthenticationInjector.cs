using Budget.Application.Abstractions.Identity;
using Budget.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Budget.API.Authentication;

internal static class AuthenticationInjector
{
    public static void InjectAuthentication(this IServiceCollection services)
    {
            services.ConfigureOptions<AuthenticationOptionsSetup>();
            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IJwtService, JwtService>();
        }
}