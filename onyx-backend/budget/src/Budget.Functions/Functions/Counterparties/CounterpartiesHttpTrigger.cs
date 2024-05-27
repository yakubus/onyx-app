using Budget.Application.Counterparties.AddCounterparty;
using Budget.Application.Counterparties.GetCounterparties;
using Budget.Application.Counterparties.RemoveCounterparty;
using Budget.Application.Counterparties.UpdateCounterparty;
using Budget.Functions.Functions.Counterparties.Requests;
using FunctionsExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Budget.Functions.Functions.Counterparties;

public sealed class CounterpartiesHttpTrigger
{
    private readonly ISender _sender;

    public CounterpartiesHttpTrigger(ISender sender)
    {
        _sender = sender;
    }

    [FunctionName(nameof(GetCounterparties))]
    public async Task<IActionResult> GetCounterparties(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "counterparties")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var type = req.Query["type"];

        var query = new GetCounterpartiesQuery(type);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(AddCounterparty))]
    public async Task<IActionResult> AddCounterparty(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "counterparties")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<AddCounterpartyRequest>(cancellationToken);

        var command = new AddCounterpartyCommand(request.CounterpartyName, request.CounterpartyType);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(UpdateCounterparty))]
    public async Task<IActionResult> UpdateCounterparty(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "counterparties/{counterpartyId}")] HttpRequest req,
        Guid counterpartyId,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<UpdateCounterpartyRequest>(cancellationToken);

        var command = new UpdateCounterpartyCommand(counterpartyId, request.NewName);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(RemoveCounterparty))]
    public async Task<IActionResult> RemoveCounterparty(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "counterparties/{counterpartyId}")] HttpRequest req,
        Guid counterpartyId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCounterpartyCommand(counterpartyId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }
}