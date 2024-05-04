using Abstractions.DomainBaseTypes;
using Budget.Domain.Accounts;
using Models.DataTypes;

namespace Budget.Domain.Transactions;

public abstract class Transaction : Entity<TransactionId>
{
    public Account Account { get; init; }
    public Money Amount { get; init; }
    public Money? OriginalAmount { get; init; }
    public DateTime TransactedAt { get; init; }

    protected Transaction(Account account, Money amount, DateTime transactedAt, Money? originalAmount)
    {
        Account = account;
        Amount = amount;
        TransactedAt = transactedAt;
        OriginalAmount = originalAmount;
    }
}