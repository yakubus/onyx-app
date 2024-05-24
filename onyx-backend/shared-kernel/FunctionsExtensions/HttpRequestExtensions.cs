using Newtonsoft.Json;

namespace FunctionsExtensions;

public static class HttpRequestExtensions
{
    public static async Task<TDestination> ConvertBodyToAsync<TDestination>(
        this Stream requestBody,
        CancellationToken cancellationToken = default)
    {
        var body = await new StreamReader(requestBody).ReadToEndAsync();

        return JsonConvert.DeserializeObject<TDestination>(body) ?? default;
    }
}