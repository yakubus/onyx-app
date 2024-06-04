using Microsoft.AspNetCore.Http;
using Models.Responses;
using Newtonsoft.Json;

namespace Budget.Infrastructure.Authorization;

internal sealed class AuthorizationErrorWriter(IHttpContextAccessor httpContextAccessor)
{
    internal async Task Write(Error error)
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return;
        }

        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(Result.Failure(error)));
    }
}