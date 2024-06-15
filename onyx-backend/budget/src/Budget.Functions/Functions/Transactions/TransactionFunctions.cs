using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.GetTransactions;
using Budget.Application.Transactions.RemoveTransaction;
using Budget.Functions.Functions.Shared;
using Budget.Functions.Functions.Transactions.Requests;
using MediatR;
using Result = Models.Responses.Result;

namespace Budget.Functions.Functions.Transactions;

public sealed class TransactionFunctions : BaseFunction
{
    private const string transactionBaseRoute = $"{BaseRouteV1}{{budgetId}}/transactions/";

    public TransactionFunctions(ISender sender) : base(sender)
    {
        
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetTransactions))]
    [HttpApi(LambdaHttpMethod.Get, transactionBaseRoute)]
    public async Task<Result> GetTransactions(
        string budgetId,
        [FromQuery] string? query,
        [FromQuery] string? counterpartyId,
        [FromQuery] string? accountId,
        [FromQuery] string? subcategoryId)
    {
        var transactionsQuery = new GetTransactionsQuery(
            query,
            counterpartyId is null ? null : Guid.Parse(counterpartyId),
            accountId is null ? null : Guid.Parse(accountId),
            subcategoryId is null ? null : Guid.Parse(subcategoryId),
            Guid.Parse(budgetId));

        var result = await Sender.Send(transactionsQuery);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(UpdateTransaction))]
    [HttpApi(LambdaHttpMethod.Post, transactionBaseRoute)]
    public async Task<Result> UpdateTransaction(
        string budgetId,
        [FromBody] AddTransactionRequest request)
    {
        var command = new AddTransactionCommand(
            request.AccountId,
            request.Amount,
            request.TransactedAt,
            request.CounterpartyName,
            request.SubcategoryId,
            Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RemoveTransaction))]
    [HttpApi(LambdaHttpMethod.Get, $"{transactionBaseRoute}{{transactionId}}")]
    public async Task<Result> RemoveTransaction(
        string budgetId,
        string transactionId)
    {
        var command = new RemoveTransactionCommand(Guid.Parse(transactionId), Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }
}