using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Budget.Functions.Extensions;
using ConfigurationProvider = Budget.Functions.Extensions.ConfigurationProvider;

namespace Budget.Functions;

internal static class Startup
{
    public static IServiceCollection ConfigureFunction()
    {
        var services = new ServiceCollection();
        var configuration = ConfigurationProvider.GetConfiguration(new ConfigurationBuilder());
        services.ConfigureServices(configuration);

        return services;
    }

    public static IServiceProvider Build(this IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}