using System.IO;
using Budget.Application;
using Budget.Functions;
using Budget.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Budget.Functions;

public sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.InjectApplication();
        builder.InjectInfrastructure();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        base.ConfigureAppConfiguration(builder);

        AddDevelopmentEnvironmentVariables(builder);
    }

    private static IConfigurationBuilder AddDevelopmentEnvironmentVariables(
        IFunctionsConfigurationBuilder builder) =>
        builder.ConfigurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
}