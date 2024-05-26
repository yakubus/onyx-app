using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Budget.Application.Transactions.GetTransactions;
using System.Threading;
using System;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.RemoveTransaction;
using Budget.Functions.Functions.Transactions.Requests;
using FunctionsExtensions;

namespace Budget.Functions.Functions.Transactions;

public sealed class TransactionsHttpTrigger
{
    private readonly ISender _sender;

    public TransactionsHttpTrigger(ISender sender)
    {
        _sender = sender;
    }

    [FunctionName("GetTransactions")]
    public async Task<IActionResult> GetTransactions(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "transactions")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        string query = req.Query["query"];
        var counterpartyId = Guid.TryParse(req.Query["counterpartyId"], out var cid) ? cid : (Guid?)null;
        var accountId = Guid.TryParse(req.Query["accountId"], out var aid) ? aid : (Guid?)null;
        var subcategoryId = Guid.TryParse(req.Query["subcategoryId"], out var sid) ? sid : (Guid?)null;

        var transactionsQuery = new GetTransactionsQuery(
            query,
            counterpartyId,
            accountId,
            subcategoryId);

        var result = await _sender.Send(transactionsQuery, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName("AddTransaction")]
    public async Task<IActionResult> AddTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "transactions")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<AddTransactionRequest>(cancellationToken);

        var command = new AddTransactionCommand(
            request.AccountId,
            request.Amount,
            request.TransactedAt,
            request.CounterpartyName,
            request.SubcategoryId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName("RemoveTransaction")]
    public async Task<IActionResult> RemoveTransaction(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "transactions/{transactionId}")] HttpRequest req,
        Guid transactionId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTransactionCommand(transactionId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }
}