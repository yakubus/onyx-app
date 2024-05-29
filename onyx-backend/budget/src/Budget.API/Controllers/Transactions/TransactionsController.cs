using Budget.API.Controllers.Transactions.Requests;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.GetTransactions;
using Budget.Application.Transactions.Models;
using Budget.Application.Transactions.RemoveTransaction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;
using Result = Models.Responses.Result;

namespace Budget.API.Controllers.Transactions;

[ApiController]
[Authorize]
[Route("/api/v1/{budgetId}/transactions")]
public sealed class TransactionsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IServiceProvider _serviceProvider;

    public TransactionsController(ISender sender, IServiceProvider serviceProvider)
    {
        _sender = sender;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<TransactionModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTransactions(
        [FromRoute] Guid budgetId,
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
            subcategoryId,
            budgetId);

        var result = await _sender.Send(transactionsQuery, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<TransactionModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(AddTransactionRequest), "application/json")]
    public async Task<IActionResult> AddTransaction(
        [FromRoute] Guid budgetId,
        [FromBody] AddTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddTransactionCommand(
            request.AccountId,
            request.Amount,
            request.TransactedAt,
            request.CounterpartyName,
            request.SubcategoryId,
            budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{transactionId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveTransaction(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTransactionCommand(transactionId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}