using Identity.API.Middlewares;

namespace Identity.API.Extensions;

internal static class WebAppConfigurator
{
    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}