using Budget.API.Controllers.Transactions.Requests;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.GetTransactions;
using Budget.Application.Transactions.RemoveTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Models.DataTypes;
using Models.Responses;
using Result = Models.Responses.Result;

namespace Budget.API.Controllers.Transactions;

[ApiController]
[Route("/api/v1/transactions")]
public sealed class TransactionsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly Error _invalidPeriodError = new (
        "AssignmentPeriod.Invalid", 
        "Invalid assignment period");

    public TransactionsController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        [FromQuery] string? query,
        [FromQuery] Guid? counterpartyId,
        [FromQuery] Guid? accountId,
        [FromQuery] Guid? subcategoryId,
        CancellationToken cancellationToken)
    {
        var transactionsQuery = new GetTransactionsQuery(
            query, 
            counterpartyId,
            accountId,
            subcategoryId);

        var result = await _sender.Send(transactionsQuery, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddTransaction(
        [FromBody] AddTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddTransactionCommand(
            request.AccountId,
            request.Amount,
            request.TransactedAt,
            request.CounterpartyName,
            request.SubcategoryId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{transactionId}")]
    public async Task<IActionResult> RemoveTransaction(
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTransactionCommand(transactionId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}