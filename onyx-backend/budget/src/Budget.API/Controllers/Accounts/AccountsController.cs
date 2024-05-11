using System.Runtime.CompilerServices;
using Budget.API.Controllers.Accounts.Requests;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers.Accounts;

[ApiController]
[Route("/api/v1/accounts")]
public sealed class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender) => _sender = sender;

    [HttpGet]
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
}