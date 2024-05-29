using Budget.API.Controllers.Accounts.Requests;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using Budget.Application.Accounts.Models;
using Budget.Application.Accounts.RemoveAccount;
using Budget.Application.Accounts.UpdateAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Accounts;

[ApiController]
[Authorize]
[Route("/api/v1/{budgetId}/accounts")]
public sealed class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<AccountModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAccounts([FromRoute] Guid budgetId, CancellationToken cancellationToken)
    {
        var query = new GetAccountsQuery(budgetId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(AddAccountRequest), "application/json")]
    public async Task<IActionResult> AddAccount(
        [FromRoute] Guid budgetId,
        [FromBody] AddAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{accountId}")]
    [ProducesResponseType(typeof(Result<AccountModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateAccountRequest), "application/json")]
    public async Task<IActionResult> UpdateAccount(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid accountId,
        [FromBody] UpdateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAccountCommand(accountId, request.NewName, request.NewBalance, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{accountId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveAccount(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAccountCommand(accountId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}