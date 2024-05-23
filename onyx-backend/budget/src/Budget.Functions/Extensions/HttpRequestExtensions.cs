using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Budget.Functions.Extensions;

internal static class HttpRequestExtensions
{
    public static async Task<T> ConvertBodyToAsync<T>(this HttpRequest request, CancellationToken cancellationToken = default)
    {
        var body = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);

        return JsonConvert.DeserializeObject<T>(body) ?? default;
    }
}