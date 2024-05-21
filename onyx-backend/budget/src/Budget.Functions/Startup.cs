using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Budget.Application;
using Budget.Functions;
using Budget.Infrastructure;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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

        builder.ConfigurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("host.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        AddSecrets(builder);
    }

    private static IConfigurationBuilder AddSecrets(IFunctionsConfigurationBuilder builder) =>
        builder.GetContext().EnvironmentName switch
        {
            var envName when envName == EnvironmentName.Development => AddDevelopmentSecrets(builder),
            var envName when envName == EnvironmentName.Production => AddProductionSecrets(builder)
        };

    private static IConfigurationBuilder AddProductionSecrets(
        IFunctionsConfigurationBuilder builder)
    {
        var vaultUri = new Uri(builder.ConfigurationBuilder.Build()["KeyVaultUri"] ??
                               throw new ConfigurationErrorsException("Missing KeyVaultUri"));

        return builder.ConfigurationBuilder
            .AddAzureKeyVault(vaultUri, new DefaultAzureCredential())
            .AddEnvironmentVariables();
    }

    private static IConfigurationBuilder AddDevelopmentSecrets(
        IFunctionsConfigurationBuilder builder) =>
        builder.ConfigurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
}