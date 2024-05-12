using Budget.API.Controllers.Counterparties.Requests;
using Budget.Application.Counterparties.AddCounterparty;
using Budget.Application.Counterparties.GetCounterparties;
using Budget.Application.Counterparties.Models;
using Budget.Application.Counterparties.RemoveCounterparty;
using Budget.Application.Counterparties.UpdateCounterparty;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Counterparties;

[ApiController]
[Route("/api/v1/counterparties")]
public sealed class CounterpartiesController : ControllerBase
{
    private readonly ISender _sender;

    public CounterpartiesController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<CounterpartyModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [EndpointDescription("Returns all counterparies of a given type (payee / payer)")]
    public async Task<IActionResult> GetCounterparties([FromQuery]string type, CancellationToken cancellationToken)
    {
        var query = new GetCounterpartiesQuery(type);

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
    [ProducesResponseType(typeof(Result<CounterpartyModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateCounterpartyRequest), "application/json")]
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
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [EndpointDescription("Removes a counterparty (not removes related transactions)")]
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