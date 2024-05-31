using Budget.API.Controllers.Budgets.Requests;
using Budget.Application.Budgets.AddBudget;
using Budget.Application.Budgets.GetBudgets;
using Budget.Application.Budgets.Models;
using Budget.Application.Budgets.RemoveBudget;
using Budget.Application.Budgets.UpdateBudget;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Budgets;

[ApiController]
[Authorize]
[Route("api/v1/budgets")]
public sealed class BudgetsController : ControllerBase
{
    private readonly ISender _sender;

    public BudgetsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateBudgetRequest), "application/json")]
    public async Task<IActionResult> GetBudgets(
        CancellationToken cancellationToken)
    {
        var command = new GetBudgetsQuery();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            NotFound(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [Consumes(typeof(AddBudgetRequest), "application/json")]
    public async Task<IActionResult> AddBudget(
    [FromBody] AddBudgetRequest request,
    CancellationToken cancellationToken)
    {
        var command = new AddBudgetCommand(request.BudgetName);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{budgetId}")]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateBudgetRequest), "application/json")]
    public async Task<IActionResult> UpdateBudget(
        [FromRoute] Guid budgetId,
        [FromBody] UpdateBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBudgetCommand(budgetId, request.UserIdToAdd, request.UserIdToRemove);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{budgetId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveBudget(
        [FromRoute] Guid budgetId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveBudgetCommand(budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}