using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.GetTransactions;
using Budget.Application.Transactions.RemoveTransaction;
using Budget.Functions.Functions.Transactions.Requests;
using MediatR;
using Result = Models.Responses.Result;

//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Transactions;

public sealed class TransactionFunctions
{
    private const string baseRoute = "/api/v1/{budgetId}/transactions/";
    private readonly ISender _sender;

    public TransactionFunctions(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> Get(
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

        var result = await _sender.Send(transactionsQuery);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> Update(
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

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, $"{baseRoute}{{transactionId}}")]
    public async Task<Result> Remove(
        string budgetId,
        string transactionId)
    {
        var command = new RemoveTransactionCommand(Guid.Parse(transactionId), Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }
}