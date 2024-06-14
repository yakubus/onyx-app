using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.APIGatewayEvents;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.GetTransactions;
using Budget.Application.Transactions.RemoveTransaction;
using Budget.Functions.Functions.Shared;
using Budget.Functions.Functions.Transactions.Requests;
using MediatR;
using Models.Exceptions;
using Newtonsoft.Json;
using Result = Models.Responses.Result;



namespace Budget.Functions.Functions.Transactions;

public sealed class TransactionFunctions : BaseFunction
{
    private const string transactionBaseRoute = $"{BaseRouteV1}{{budgetId}}/transactions/";

    public TransactionFunctions(ISender sender) 
        : base(sender)
    {

    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Get, transactionBaseRoute)]
    public async Task<Result> Get(APIGatewayHttpApiV2ProxyRequest request)
    {
        var budgetId = request.PathParameters["budgetId"];
        var query = request.QueryStringParameters["query"];
        var counterpartyId = request.QueryStringParameters["counterpartyId"];
        var accountId = request.QueryStringParameters["accountId"];
        var subcategoryId = request.QueryStringParameters["subcategoryId"];

        Guid? parsedCounterpartyId = string.IsNullOrWhiteSpace(counterpartyId) ?
            null :
            Guid.Parse(counterpartyId);
        Guid? parsedAccountId = string.IsNullOrWhiteSpace(accountId) ? 
            null : 
            Guid.Parse(accountId);
        Guid? parsedSubcategoryId = string.IsNullOrWhiteSpace(subcategoryId) ?
            null :
            Guid.Parse(subcategoryId);

        var transactionsQuery = new GetTransactionsQuery(
            query,
            parsedCounterpartyId,
            parsedAccountId,
            parsedSubcategoryId,
            Guid.Parse(budgetId));

        var result = await Sender.Send(transactionsQuery);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Post, transactionBaseRoute)]
    public async Task<Result> Update(APIGatewayHttpApiV2ProxyRequest request)
    {
        var budgetId = request.PathParameters["budgetId"];

        var requestBody = JsonConvert.DeserializeObject<AddTransactionRequest>(request.Body) ??
                          throw new InvalidBodyRequestException(typeof(AddTransactionRequest));

        var command = new AddTransactionCommand(
            requestBody.AccountId,
            requestBody.Amount,
            requestBody.TransactedAt,
            requestBody.CounterpartyName,
            requestBody.SubcategoryId,
            Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole)]
    [HttpApi(LambdaHttpMethod.Get, $"{transactionBaseRoute}{{transactionId}}")]
    public async Task<Result> Remove(APIGatewayHttpApiV2ProxyRequest request)
    {
        var budgetId = request.PathParameters["budgetId"];
        var transactionId = request.PathParameters["transactionId"];

        var command = new RemoveTransactionCommand(Guid.Parse(transactionId), Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }
}