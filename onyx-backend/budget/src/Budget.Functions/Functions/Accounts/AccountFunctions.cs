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
    public async Task<Result> GetAccounts(Guid budgetId)
    {
        var query = new GetAccountsQuery(budgetId);

        var result = await _sender.Send(query);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> AddAccount(
        Guid budgetId,
        [FromBody] AddAccountRequest request)
    {
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{accountId}}")]
    public async Task<Result> UpdateAccount(
        Guid budgetId,
        Guid accountId,
        [FromBody] UpdateAccountRequest request)
    {
        var command = new UpdateAccountCommand(accountId, request.NewName, request.NewBalance, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{accountId}}")]
    public async Task<Result> RemoveAccount(
        Guid budgetId,
        Guid accountId)
    {
        var command = new RemoveAccountCommand(accountId, budgetId);

        var result = await _sender.Send(command);

        return result;
    }
}