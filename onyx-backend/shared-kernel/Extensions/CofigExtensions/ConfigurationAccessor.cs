using Microsoft.Extensions.Configuration;

namespace Extensions.CofigExtensions;

public static class ConfigurationAccessor
{
    private const string baseProductionKey = "AzureFunctionsJobHost:";
    private const string baseDevelopmentKey = "???:";

    // TODO create second for development
    public static IConfigurationSection GetFunctionSection(this IConfiguration configuration, string key)
    {
        return configuration.GetSection(string.Concat(baseProductionKey, key));
    }
}