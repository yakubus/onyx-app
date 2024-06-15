using Budget.Application;
using Budget.Functions.Documentation;
using Budget.Functions.Logger;
using Budget.Functions.Middlewares;
using Budget.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#pragma warning disable CS1591

namespace Budget.Functions;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = UseConfiguration(services);
        services.AddLogger();
        services.InjectApplication();
        services.InjectInfrastructure(configuration);
        services.Document();

        //// Add AWS Systems Manager as a potential provider for the configuration. This is 
        //// available with the Amazon.Extensions.Configuration.SystemsManager NuGet package.
        //builder.AddSystemsManager("/app/settings");

        //// Example of using the AWSSDK.Extensions.NETCore.Setup NuGet package to add
        //// the Amazon S3 service client to the dependency injection container.
        //services.AddAWSService<Amazon.S3.IAmazonS3>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "Customers API V1");
            c.RoutePrefix = "swagger";
        });

        app.UseEndpoints(endpoints => endpoints.MapControllers());

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
