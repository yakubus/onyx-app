using Budget.Infrastructure.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Infrastructure;

public static class DependencyInjection
{
    public static void InjectInfrastructure(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddPersistence(builder.GetContext().Configuration);
    }

    //TODO Temporary solution
    public static IServiceCollection InjectInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);


        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDbOptions>(configuration.GetSection("CosmosDb"));
        services.AddScoped<CosmosDbContext>();
    }

    private static void AddCurrencyConverter(this IServiceCollection services, IConfiguration configuration)
    {
        
    }
}