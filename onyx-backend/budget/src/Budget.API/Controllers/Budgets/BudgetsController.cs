using Budget.API.Controllers.Budgets.Requests;
using Budget.Application.Budgets.AddBudget;
using Budget.Application.Budgets.GetBudgetDetails;
using Budget.Application.Budgets.GetBudgetInvitation;
using Budget.Application.Budgets.GetBudgets;
using Budget.Application.Budgets.Models;
using Budget.Application.Budgets.RemoveBudget;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;
using System.IO;
using Budget.Application.Budgets.AddUserToBudget;
using Budget.Application.Budgets.RemoveUserFromBudgetBudget;

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
    [Consumes(typeof(RemoveUserFromBudgetRequest), "application/json")]
    public async Task<IActionResult> GetBudgets(
        CancellationToken cancellationToken)
    {
        var command = new GetBudgetsQuery();

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            NotFound(result);
    }

    [HttpGet("{budgetId}")]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(RemoveUserFromBudgetRequest), "application/json")]
    public async Task<IActionResult> GetBudgetDetails(
        [FromRoute] Guid budgetId,
        CancellationToken cancellationToken)
    {
        var command = new GetBudgetDetailsQuery(budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            NotFound(result);
    }

    [HttpPut("{budgetId}/invitation")]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(InvitationUrl), "application/json")]
    public async Task<IActionResult> GetBudgetInvitation(
        [FromRoute] Guid budgetId,
        CancellationToken cancellationToken)
    {
        var protocol = Request.Protocol;
        var host = Request.Host.Value;
        var baseUrl = $"{protocol}://{host}";
        var command = new GetBudgetInvitationQuery(budgetId, baseUrl);

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
        var command = new AddBudgetCommand(request.BudgetName, request.BudgetCurrency);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{budgetId}/remove/{userId}")]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(RemoveUserFromBudgetRequest), "application/json")]
    public async Task<IActionResult> RemoveUserFromBudget(
        [FromRoute] Guid budgetId,
        [FromRoute] string userId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveUserFromBudgetCommand(budgetId, userId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{budgetId}/join/{token}")]
    [ProducesResponseType(typeof(Result<BudgetModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(RemoveUserFromBudgetRequest), "application/json")]
    public async Task<IActionResult> JoinTheBudget(
        [FromRoute] Guid budgetId,
        [FromRoute] string token,
        CancellationToken cancellationToken)
    {
        var command = new AddUserToBudgetCommand(budgetId, token);

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