using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.DomainBaseTypes;
using Budget.Domain.Accounts;
using Budget.Domain.Subcategories;
using Models.DataTypes;

namespace Budget.Domain.Transactions;

public abstract class Transaction : Entity<TransactionId>
{
    public Account Account { get; init; }
    public Subcategory Subcategory { get; init; }
    public Money Amount { get; init; }
    public Money? OriginalAmount { get; init; }
    public DateTime TransactedAt { get; init; }

    protected Transaction(Account account, Subcategory subcategory, Money amount, DateTime transactedAt, Money? originalAmount)
    {
        Account = account;
        Subcategory = subcategory;
        Amount = amount;
        TransactedAt = transactedAt;
        OriginalAmount = originalAmount;
    }
}