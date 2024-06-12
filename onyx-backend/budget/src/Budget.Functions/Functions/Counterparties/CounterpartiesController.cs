using Budget.Application.Counterparties.AddCounterparty;
using Budget.Application.Counterparties.GetCounterparties;
using Budget.Application.Counterparties.Models;
using Budget.Application.Counterparties.RemoveCounterparty;
using Budget.Application.Counterparties.UpdateCounterparty;
using Budget.Functions.Functions.Counterparties.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.Functions.Functions.Counterparties;

[ApiController]
[Authorize]
[Route("/api/v1/{budgetId}/counterparties")]
public sealed class CounterpartiesController : ControllerBase
{
    private readonly ISender _sender;

    public CounterpartiesController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<CounterpartyModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCounterparties(
        [FromRoute] Guid budgetId,
        [FromQuery] string type,
        CancellationToken cancellationToken)
    {
        var query = new GetCounterpartiesQuery(type, budgetId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<CounterpartyModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(AddCounterpartyRequest), "application/json")]
    public async Task<IActionResult> AddCounterparty(
        [FromRoute] Guid budgetId,
        [FromBody] AddCounterpartyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddCounterpartyCommand(request.CounterpartyType, request.CounterpartyName, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{counterpartyId}")]
    [ProducesResponseType(typeof(Result<CounterpartyModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateCounterpartyRequest), "application/json")]
    public async Task<IActionResult> UpdateCounterparty(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid counterpartyId,
        [FromBody] UpdateCounterpartyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCounterpartyCommand(counterpartyId, request.NewName, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{counterpartyId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveCounterparty(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid counterpartyId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCounterpartyCommand(counterpartyId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}