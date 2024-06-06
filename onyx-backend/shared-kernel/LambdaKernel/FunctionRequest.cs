using System.Text.RegularExpressions;
using Amazon.Lambda.APIGatewayEvents;
using Models.Exceptions;
using Newtonsoft.Json;

namespace LambdaKernel;

public record FunctionRequest
{
    public APIGatewayProxyRequest BaseRequest { get; private set; }
    public string Path => BaseRequest.Path;
    public object? Body => JsonConvert.DeserializeObject(BaseRequest.Body);

    public FunctionRequest(APIGatewayProxyRequest baseRequest) =>
        BaseRequest = baseRequest;

    public string FromRoute(Regex regex)
    {
        var match = regex.Match(Path);

        if (!match.Success)
        {
            throw new InvalidRouteRequestException(Path, regex);
        }

        var routeParameter = match.Groups["budgetId"].Value;
        return routeParameter;
    }
}

public sealed record FunctionRequest<TBody> : FunctionRequest
{
    public TBody Body => (TBody?)base.Body
                         ?? throw new InvalidBodyRequestException(typeof(TBody));

    public FunctionRequest(APIGatewayProxyRequest baseRequest) : base(baseRequest)
    { }
}
