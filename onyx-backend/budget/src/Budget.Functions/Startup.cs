using Budget.Application;
using Budget.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

namespace Budget.Functions;

public sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.InjectApplication();
        builder.InjectInfrastructure();
    }
}