using Abstractions.Messaging;

namespace Budget.Application.Transactions.RemoveTransaction;

public sealed record RemoveTransactionCommand(Guid TransactionId) : ICommand
{
}