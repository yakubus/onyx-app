using AWS.Logger;
using AWS.Logger.SeriLog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Identity.Functions.Logger;

internal static class LoggerInjector
{
    public static IServiceCollection AddLogger(this IServiceCollection services)
    {
        var config = new AWSLoggerConfig
        {
            Region = LoggerOptions.Region,
            LogGroup = LoggerOptions.LogGroup
        };

        var logger = new LoggerConfiguration()
            .WriteTo.AWSSeriLog(config)
            .CreateLogger();

        services.AddSingleton<ILogger>(logger);

        return services;
    }
}