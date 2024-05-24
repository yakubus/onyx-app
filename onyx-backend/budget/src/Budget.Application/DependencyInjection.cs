using Budget.Application.Behaviors;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Application;

public static class DependencyInjection
{
    public static IFunctionsHostBuilder InjectApplication(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddMediatR(
            config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

        return builder;
    }
}