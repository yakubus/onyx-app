using System.IO;
using System.Threading.Tasks;
using Budget.Functions.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Budget.Application.Transactions.GetTransactions;
using System.Threading;
using System;
using Microsoft.Extensions.Primitives;

namespace Budget.Functions.Functions.Transactions;

public sealed class HttpTrigger
{
    private readonly ISender _sender;

    public HttpTrigger(ISender sender)
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

    private Guid? ParseGuid(string id) => Guid.TryParse(id, out var guid) ? guid : null;
}