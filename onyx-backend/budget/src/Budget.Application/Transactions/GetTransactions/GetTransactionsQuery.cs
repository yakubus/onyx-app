using Abstractions.Messaging;
using Budget.Application.Transactions.Models;

namespace Budget.Application.Transactions.GetTransactions;

public sealed record GetTransactionsQuery(
    string? Query,
    Guid? CounterpartyId,
    Guid? AccountId,
    Guid? SubcategoryId) : IQuery<IEnumerable<TransactionModel>>
{
}