using Abstractions.Messaging;
using Budget.Application.Shared.Models;
using Budget.Application.Transactions.Models;

namespace Budget.Application.Transactions.AddTransaction;

public sealed record AddTransactionCommand(
    Guid AccountId,
    MoneyModel Amount,
    DateTime TransactedAt,
    string CounterpartyName,
    Guid? SubcategoryId) : ICommand<TransactionModel>
{
}