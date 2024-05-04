using System.Text.Json.Serialization;
using Abstractions.DomainBaseTypes;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Accounts;

public abstract class Account : Entity<AccountId>
{
    public AccountName Name { get; private set; }
    public Money Balance { get; private set; }

    protected Account(AccountName name, Money balance)
    {
        Name = name;
        Balance = balance;
    }

    [JsonConstructor]
    protected Account()
    { }

    public Result ChangeName(string name)
    {
        var accountNameCreateResult = AccountName.Create(name);

        if(accountNameCreateResult.IsFailure)
        {
            return Result.Failure(accountNameCreateResult.Error);
        }

        var accountName = accountNameCreateResult.Value;
        Name = accountName;

        return Result.Success();
    }

    public Result ChangeBalance(Money balance)
    {
        Balance = balance;

        return Result.Success();
    }

    internal Result Transact(Transaction transaction)
    {
        if(transaction.Amount.Currency != Balance.Currency)
        {
            return Result.Failure(AccountErrors.InconsistentCurrency);
        }

        Balance += transaction.Amount;

        return Result.Success();
    }

    public Result RemoveTransaction(Transaction transaction)
    {
        if (transaction.Amount.Currency != Balance.Currency)
        {
            return Result.Failure(AccountErrors.InconsistentCurrency);
        }

        Balance -= transaction.Amount;

        return Result.Success();
    }
}