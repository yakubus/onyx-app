using Microsoft.Extensions.Configuration;

namespace Budget.Functions.Extensions;

internal static class ConfigurationProvider
{
    private const string configurationFileName = "appsettings.json";

    public static IConfiguration GetConfiguration(ConfigurationBuilder configurationBuilder) =>
        configurationBuilder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configurationFileName, optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
}