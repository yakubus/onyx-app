using Budget.API.Authentication;
using Budget.Application;
using Budget.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Budget.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Onyx", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        builder.Services.InjectApplication();
        builder.Services.InjectInfrastructure(builder.Configuration);
        builder.Services.InjectAuthentication();
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

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}