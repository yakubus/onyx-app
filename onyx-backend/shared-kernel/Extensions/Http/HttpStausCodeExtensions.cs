using System.Net;

namespace Extensions.Http;

public static class HttpStausCodeExtensions
{
    public static bool IsSuccessful(this HttpStatusCode code) =>
        (int)code > 199 && (int)code < 300;
}