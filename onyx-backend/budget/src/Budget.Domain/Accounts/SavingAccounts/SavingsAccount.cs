using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Accounts.SavingAccounts;

public sealed class SavingsAccount : Account
{
    private SavingsAccount(AccountName name, Money balance)
        : base(name, balance)
    { }

    public static Result<Account> Create(string name, Money balance)
    {
        var accountNameCreateResult = AccountName.Create(name);

        if (accountNameCreateResult.IsFailure)
        {
            return Result.Failure<Account>(accountNameCreateResult.Error);
        }

        var accountName = accountNameCreateResult.Value;

        return new SavingsAccount(accountName, balance);
    }
}