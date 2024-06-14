using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Amazon.Util.Internal.PlatformServices;
using Budget.Application.Counterparties.AddCounterparty;
using Budget.Application.Counterparties.GetCounterparties;
using Budget.Application.Counterparties.RemoveCounterparty;
using Budget.Application.Counterparties.UpdateCounterparty;
using Budget.Functions.Functions.Counterparties.Requests;
using Budget.Functions.Functions.Shared;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Models.Responses;



namespace Budget.Functions.Functions.Counterparties;

public sealed class CounterpartyFunctions : BaseFunction
{
    private const string counterpartyBaseRoute = $"{BaseRouteV1}{{budgetId}}/counterparties/";

    public CounterpartyFunctions(ISender sender) : base(sender)
    {
    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Get, counterpartyBaseRoute)]
    public async Task<Result> GetAll(
        Guid budgetId,
        [FromQuery] string type)
    {
        var query = new GetCounterpartiesQuery(type, budgetId);

        var result = await Sender.Send(query);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Post, counterpartyBaseRoute)]
    public async Task<Result> Add(
        Guid budgetId,
        [FromBody] AddCounterpartyRequest request)
    {
        var command = new AddCounterpartyCommand(
            request.CounterpartyType,
            request.CounterpartyName,
            budgetId);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Put, $"{counterpartyBaseRoute}{{counterpartyId}}")]
    public async Task<Result> Update(
        string budgetId,
        string counterpartyId,
        [FromBody] UpdateCounterpartyRequest request)
    {
        var command = new UpdateCounterpartyCommand(
            Guid.Parse(counterpartyId),
            request.NewName,
            Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Delete, $"{counterpartyBaseRoute}{{counterpartyId}}")]
    public async Task<Result> Remove(
        string budgetId,
        string counterpartyId)
    {
        var command = new RemoveCounterpartyCommand(Guid.Parse(counterpartyId), Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }
}