using System;
using System.Threading;
using System.Threading.Tasks;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using Budget.Application.Accounts.RemoveAccount;
using Budget.Application.Accounts.UpdateAccount;
using Budget.Functions.Functions.Accounts.Requests;
using FunctionsExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Budget.Functions.Functions.Accounts;

public sealed class AccountsHttpTrigger
{
    private readonly ISender _sender;

    public AccountsHttpTrigger(ISender sender)
    {
        _sender = sender;
    }

    [FunctionName(nameof(GetAccounts))]
    public async Task<IActionResult> GetAccounts(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var query = new GetAccountsQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(AddAccount))]
    public async Task<IActionResult> AddAccount(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "accounts")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<AddAccountRequest>(cancellationToken);
        
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(UpdateAccount))]
    public async Task<IActionResult> UpdateAccount(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "accounts/{accountId}")] HttpRequest req,
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<UpdateAccountRequest>(cancellationToken);

        var command = new UpdateAccountCommand(accountId, request.NewName, request.NewBalance);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(RemoveAccount))]
    public async Task<IActionResult> RemoveAccount(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "accounts/{accountId}")] HttpRequest req,
        Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAccountCommand(accountId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }
}