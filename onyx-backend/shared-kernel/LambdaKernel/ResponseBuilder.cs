using Amazon.Lambda.APIGatewayEvents;
using Models.Responses;
using Newtonsoft.Json;

namespace LambdaKernel;

public static class ResponseBuilder
{
    private static APIGatewayProxyResponse Respond(Result result, int statusCode, Dictionary<string, string>? headers = null) =>
        new()
        {
            StatusCode = statusCode,
            Headers = headers ?? new Dictionary<string, string>(),
            Body = JsonConvert.SerializeObject(result),
        };
    private static APIGatewayProxyResponse Respond<T>(Result<T> result, int statusCode, Dictionary<string, string>? headers = null) =>
        new()
        {
            StatusCode = statusCode,
            Headers = headers ?? new Dictionary<string, string>(),
            Body = JsonConvert.SerializeObject(result),
        };

    public static APIGatewayProxyResponse ReturnAPIResponse(
        this Result result,
        int? successStatusCode = null,
        int? failureStatusCode = null)
    {
        return result.IsSuccess ?
            Respond(result, successStatusCode ?? 200) :
            Respond(result, failureStatusCode ?? 400);
    }

    public static APIGatewayProxyResponse ReturnAPIResponse<T>(
        this Result<T> result,
        int? successStatusCode = null,
        int? failureStatusCode = null)
    {
        return result.IsSuccess ?
            Respond(result, successStatusCode ?? 200) :
            Respond(result, failureStatusCode ?? 400);
    }
}