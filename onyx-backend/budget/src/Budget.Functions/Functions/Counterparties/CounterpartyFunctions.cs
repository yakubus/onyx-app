using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Counterparties.AddCounterparty;
using Budget.Application.Counterparties.GetCounterparties;
using Budget.Application.Counterparties.RemoveCounterparty;
using Budget.Application.Counterparties.UpdateCounterparty;
using Budget.Functions.Functions.Counterparties.Requests;
using MediatR;
using Models.Responses;

//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Counterparties;

public sealed class CounterpartyFunctions
{
    private const string baseRoute = "/api/v1/{budgetId}/counterparties/";
    private readonly ISender _sender;

    public CounterpartyFunctions(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetCounterparties(
        Guid budgetId,
        [FromQuery] string type)
    {
        var query = new GetCounterpartiesQuery(type, budgetId);

        var result = await _sender.Send(query);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> AddCounterparty(
        Guid budgetId,
        [FromBody] AddCounterpartyRequest request)
    {
        var command = new AddCounterpartyCommand(request.CounterpartyType, request.CounterpartyName, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{counterpartyId}}")]
    public async Task<Result> UpdateCounterparty(
        Guid budgetId,
        Guid counterpartyId,
        [FromBody] UpdateCounterpartyRequest request)
    {
        var command = new UpdateCounterpartyCommand(counterpartyId, request.NewName, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{counterpartyId}}")]
    public async Task<Result> RemoveCounterparty(
        Guid budgetId,
        Guid counterpartyId)
    {
        var command = new RemoveCounterpartyCommand(counterpartyId, budgetId);

        var result = await _sender.Send(command);

        return result;
    }
}