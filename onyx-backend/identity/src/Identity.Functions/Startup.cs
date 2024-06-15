using Identity.Application;
using Identity.Functions.Logger;
using Identity.Functions.Middlewares;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Functions;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = UseConfiguration(services);
        services.AddLogger();
        services.InjectApplication();
        services.InjectInfrastructure(configuration);

        //// Add AWS Systems Manager as a potential provider for the configuration. This is 
        //// available with the Amazon.Extensions.Configuration.SystemsManager NuGet package.
        //builder.AddSystemsManager("/app/settings");

        //// Example of using the AWSSDK.Extensions.NETCore.Setup NuGet package to add
        //// the Amazon S3 service client to the dependency injection container.
        //services.AddAWSService<Amazon.S3.IAmazonS3>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }

    private static IConfiguration UseConfiguration(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        return configuration;
    }
}
