using Budget.API.Middlewares;

namespace Budget.API.Extensions;

internal static class WebAppConfigurator
{
    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}