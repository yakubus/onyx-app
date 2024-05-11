using Abstractions.DomainBaseTypes;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Accounts;

public sealed class Account : Entity<AccountId>
{
    public AccountName Name { get; private set; }
    public Money Balance { get; private set; }
    public AccountType Type { get; init; }

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Account(AccountName name, Money balance, AccountType type) : base(new AccountId())
    {
        Name = name;
        Balance = balance;
        Type = type;
    }

    public static Result<Account> Create(string name, Money balance, string type)
    {
        var accountNameCreateResult = AccountName.Create(name);

        if (accountNameCreateResult.IsFailure)
        {
            return Result.Failure<Account>(accountNameCreateResult.Error);
        }

        var accountName = accountNameCreateResult.Value;

        var accountTypeCreateResult = AccountType.Create(type);

        if (accountTypeCreateResult.IsFailure)
        {
            return Result.Failure<Account>(accountTypeCreateResult.Error);
        }

        var accountType = accountTypeCreateResult.Value;

        return new Account(accountName, balance, accountType);
    }

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