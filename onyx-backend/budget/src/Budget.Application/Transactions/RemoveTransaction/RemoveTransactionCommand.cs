using Budget.Application.Abstractions.Messaging;

namespace Budget.Application.Transactions.RemoveTransaction;

public sealed record RemoveTransactionCommand(Guid TransactionId, Guid BudgetId) : BudgetCommand(BudgetId)
{
}