using System;
using System.IO;
using Identity.Application;
using Identity.Functions;
using Identity.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Identity.Functions;

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
            var envName when envName == Environments.Development => AddDevelopmentSecrets(builder),
            //var envName when envName == Environments.Production => AddProductionSecrets(builder),
            var envName => throw new ArgumentException($"Invalid environment - {envName}")
        };
    // No need for now
    //private static IConfigurationBuilder AddProductionSecrets(
    //    IFunctionsConfigurationBuilder builder)
    //{
    //    var vaultUri = new Uri(builder.ConfigurationBuilder.Build()["KeyVaultUri"] ??
    //                           throw new ConfigurationErrorsException("Missing KeyVaultUri"));

    //    return builder.ConfigurationBuilder
    //        .AddAzureKeyVault(vaultUri, new DefaultAzureCredential())
    //        .AddEnvironmentVariables();
    //}

    private static IConfigurationBuilder AddDevelopmentSecrets(
        IFunctionsConfigurationBuilder builder) =>
        builder.ConfigurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
}