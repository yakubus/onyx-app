using Identity.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAWSLambdaHosting(LambdaEventSource.HttpApi)
    .ConfigureServices(builder.Configuration)
    .ConfigureCors()
    .AddDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewares()
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();
