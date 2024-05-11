using Budget.API.Controllers.Counterparties.Requests;
using Budget.Application.Counterparties.AddCounterparty;
using Budget.Application.Counterparties.GetCounterparties;
using Budget.Application.Counterparties.RemoveCounterparty;
using Budget.Application.Counterparties.UpdateCounterparty;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers.Counterparties;

[ApiController]
[Route("/api/v1/counterparties")]
public sealed class CounterpartiesController : ControllerBase
{
    private readonly ISender _sender;

    public CounterpartiesController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetCounterparties([FromQuery]string type, CancellationToken cancellationToken)
    {
        var query = new GetCounterpartiesQuery(type);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCounterparty(
        [FromBody] AddCounterpartyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddCounterpartyCommand(request.CounterpartyType, request.CounterpartyName);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{counterpartyId}")]
    public async Task<IActionResult> UpdateCounterparty(
        [FromRoute] Guid counterpartyId,
        [FromBody] UpdateCounterpartyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCounterpartyCommand(counterpartyId, request.NewName);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{counterpartyId}")]
    public async Task<IActionResult> RemoveCounterparty(
        [FromRoute] Guid counterpartyId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCounterpartyCommand(counterpartyId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}