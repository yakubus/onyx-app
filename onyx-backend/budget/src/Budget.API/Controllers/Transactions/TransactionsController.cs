using Budget.API.Controllers.Transactions.Requests;
using Budget.Application.Transactions.AddTransaction;
using Budget.Application.Transactions.GetTransactions;
using Budget.Application.Transactions.Models;
using Budget.Application.Transactions.RemoveTransaction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Models.Responses;
using Result = Models.Responses.Result;

namespace Budget.API.Controllers.Transactions;

[ApiController]
[Route("/api/v1/transactions")]
public sealed class TransactionsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IConfiguration _configuration;

    public TransactionsController(ISender sender, IConfiguration configuration)
    {
        _sender = sender;
        _configuration = configuration;
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<TransactionModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [EndpointDescription(
        "Returns all transactions for a given query (all, counterparty, account, subcategory)")]
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
    [ProducesResponseType(typeof(Result<TransactionModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(AddTransactionRequest), "application/json")]
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
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
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