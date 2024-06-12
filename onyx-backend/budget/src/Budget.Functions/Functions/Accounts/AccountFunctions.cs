using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using Budget.Application.Accounts.RemoveAccount;
using Budget.Application.Accounts.UpdateAccount;
using Budget.Functions.Functions.Accounts.Requests;
using MediatR;
using Models.Responses;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Accounts;

public sealed class AccountFunctions
{
    private const string baseRoute = "/api/v1/{budgetId}/accounts/";
    private readonly ISender _sender;

    public AccountFunctions(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetAccounts([FromRoute] Guid budgetId, CancellationToken cancellationToken)
    {
        var query = new GetAccountsQuery(budgetId);

        var result = await _sender.Send(query, cancellationToken);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> AddAccount(
        [FromRoute] Guid budgetId,
        [FromBody] AddAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{accountId}}")]
    public async Task<Result> UpdateAccount(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid accountId,
        [FromBody] UpdateAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAccountCommand(accountId, request.NewName, request.NewBalance, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{accountId}}")]
    public async Task<Result> RemoveAccount(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid accountId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAccountCommand(accountId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }
}