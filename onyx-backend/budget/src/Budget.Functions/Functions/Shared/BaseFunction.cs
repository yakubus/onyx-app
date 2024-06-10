using Amazon.Lambda.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Amazon.Lambda.APIGatewayEvents;

namespace Budget.Functions.Functions.Shared;

public abstract class BaseFunction
{
    protected readonly ISender Sender;

    protected BaseFunction()
    {
        var serviceProvider = Startup.ConfigureFunction().Build();
        Sender = serviceProvider.GetRequiredService<ISender>();
    }

    protected abstract Task<APIGatewayProxyResponse> HandleAsync(
        APIGatewayProxyRequest functionRequest,
        ILambdaContext context,
        CancellationToken cancellationToken);
}

