using Budget.Application;
using Budget.Infrastructure;

namespace Budget.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.InjectApplication();
        builder.Services.InjectInfrastructure(builder.Configuration);

        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}