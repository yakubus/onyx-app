using Amazon.Lambda.APIGatewayEvents;
using Models.Responses;
using Newtonsoft.Json;

namespace LambdaKernel;

public static class ResponseBuilder
{
    private static APIGatewayHttpApiV2ProxyResponse Respond(Result result, int statusCode, Dictionary<string, string>? headers = null) =>
        new()
        {
            StatusCode = statusCode,
            Headers = headers ?? new Dictionary<string, string>(),
            Body = JsonConvert.SerializeObject(result),
        };
    private static APIGatewayHttpApiV2ProxyResponse Respond<T>(Result<T> result, int statusCode, Dictionary<string, string>? headers = null) =>
        new()
        {
            StatusCode = statusCode,
            Headers = headers ?? new Dictionary<string, string>(),
            Body = JsonConvert.SerializeObject(result),
        };

    public static APIGatewayHttpApiV2ProxyResponse ReturnAPIResponse(
        this Result result,
        int? successStatusCode = null,
        int? failureStatusCode = null,
        Dictionary<string, string>? headers = null)
    {
        return result.IsSuccess ?
            Respond(result, successStatusCode ?? 200, headers) :
            Respond(result, failureStatusCode ?? 400, headers);
    }

    public static APIGatewayHttpApiV2ProxyResponse ReturnAPIResponse<T>(
        this Result<T> result,
        int? successStatusCode = null,
        int? failureStatusCode = null,
        Dictionary<string, string>? headers = null)
    {
        return result.IsSuccess ?
            Respond(result, successStatusCode ?? 200, headers) :
            Respond(result, failureStatusCode ?? 400, headers);
    }
}