using Abstractions.Messaging;
using Amazon.Lambda.Core;
using Budget.Application.Abstractions.Currency;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Models.DataTypes;
using Models.Responses;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions;

public class Function
{
    private readonly ISender _sender;

    public Function()
    {
        var serviceProvider = Startup.ConfigureFunction().Build();
        _sender = serviceProvider.GetRequiredService<ISender>();
    }

    public async Task<string> FunctionHandler(ILambdaContext context)
    {
        return "Hello World!";
    }

}
