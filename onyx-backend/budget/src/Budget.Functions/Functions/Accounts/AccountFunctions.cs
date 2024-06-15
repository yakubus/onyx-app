using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using Budget.Application.Accounts.RemoveAccount;
using Budget.Application.Accounts.UpdateAccount;
using Budget.Functions.Functions.Accounts.Requests;
using Budget.Functions.Functions.Shared;
using MediatR;
using Models.Responses;
#pragma warning disable CS1591

namespace Budget.Functions.Functions.Accounts;

public sealed class AccountFunctions : BaseFunction
{
    private const string accountsBaseRoute = $"{BaseRouteV1}{{budgetId}}/accounts/";

    public AccountFunctions(ISender sender) : base(sender)
    {
        
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetAllAccounts))]
    [HttpApi(LambdaHttpMethod.Get, accountsBaseRoute)]
    public async Task<Result> GetAllAccounts(string budgetId)
    {
        var query = new GetAccountsQuery(Guid.Parse(budgetId));

        var result = await Sender.Send(query);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(AddAccount))]
    [HttpApi(LambdaHttpMethod.Post, accountsBaseRoute)]
    public async Task<Result> AddAccount(
        string budgetId,
        [Amazon.Lambda.Annotations.APIGateway.FromBody] AddAccountRequest request)
    {
        var command = new AddAccountCommand(request.Name, request.Balance, request.AccountType, Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(UpdateAccount))]
    [HttpApi(LambdaHttpMethod.Put, $"{accountsBaseRoute}{{accountId}}")]
    public async Task<Result> UpdateAccount(
        string budgetId,
        string accountId,
        [Amazon.Lambda.Annotations.APIGateway.FromBody] UpdateAccountRequest request)
    {
        var command = new UpdateAccountCommand(Guid.Parse(accountId), request.NewName, request.NewBalance, Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RemoveAccount))]
    [HttpApi(LambdaHttpMethod.Delete, $"{accountsBaseRoute}{{accountId}}")]
    public async Task<Result> RemoveAccount(
        string budgetId,
        string accountId)
    {
        var command = new RemoveAccountCommand(Guid.Parse(accountId), Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }
}