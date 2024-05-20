using Budget.Application;
using Budget.Infrastructure;

namespace Budget.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApp(builder.Configuration.GetFunctionSection("AzureAd"));

        //builder.Services.AddTokenAcquisition().AddInMemoryTokenCaches();

        //builder.Services.AddAuthorization(options =>
        //{
        //    options.FallbackPolicy = options.DefaultPolicy;
        //});

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.InjectApplication();
        builder.Services.InjectInfrastructure(builder.Configuration);
        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DefaultPolicy",
                b =>
                {
                    b.WithOrigins(
                            "http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}