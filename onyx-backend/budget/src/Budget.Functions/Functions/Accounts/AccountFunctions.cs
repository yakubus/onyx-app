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
    public async Task<Result> GetAll(string budgetId)
    {
        var query = new GetAccountsQuery(Guid.Parse(budgetId));

        var result = await _sender.Send(query);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> Add(
        string budgetId,
        [FromBody] AddAccountRequest request)
    {
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType, Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{accountId}}")]
    public async Task<Result> Update(
        string budgetId,
        string accountId,
        [FromBody] UpdateAccountRequest request)
    {
        var command = new UpdateAccountCommand(Guid.Parse(accountId), request.NewName, request.NewBalance, Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{accountId}}")]
    public async Task<Result> Remove(
        string budgetId,
        string accountId)
    {
        var command = new RemoveAccountCommand(Guid.Parse(accountId), Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }
}