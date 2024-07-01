using Amazon.Lambda.Core;
using MediatR;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Shared;

public abstract class BaseFunction
{
    protected const string FullAccessRole = "arn:aws:iam::975049887576:role/FullAccess";
    protected const string BaseRouteV1 = "/api/v1";
    protected readonly ISender Sender;

    protected BaseFunction(ISender sender) => Sender = sender;
}