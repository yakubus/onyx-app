using System.Runtime.CompilerServices;
using Budget.API.Controllers.Accounts.Requests;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using Budget.Application.Accounts.Models;
using Budget.Application.Accounts.RemoveAccount;
using Budget.Application.Accounts.UpdateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Accounts;

[ApiController]
[Route("/api/v1/accounts")]
public sealed class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<AccountModel>>), 200)]
    [ProducesResponseType(typeof(Result<IEnumerable<AccountModel>>), 400)]
    public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
    {
        var query = new GetAccountsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAccount(
        [FromBody] AddAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{accountId}")]
    public async Task<IActionResult> UpdateAccount(
        [FromRoute] Guid accountId,
        [FromBody] UpdateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAccountCommand(accountId, request.NewName, request.NewBalance);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> RemoveAccount(
        [FromRoute] Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAccountCommand(accountId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}