using Budget.Domain.Accounts;
using Budget.Domain.Counterparties.Payees;
using Budget.Domain.Counterparties.Payers;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions.Inflows;

public sealed class Inflow : Transaction
{
    public Payer Payer { get; init; }

    private Inflow(Account account, Money amount, DateTime transactedAt, Payer payer, Money? originalAmount) 
        : base(account, amount, transactedAt, originalAmount)
    {
        Payer = payer;
    }

    public static Result<Inflow> CreatePrincipal(
        Account account,
        Money amount,
        DateTime transactedAt,
        Payer payer)
    {
        var transaction = new Inflow(account, amount, transactedAt, payer, null);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Inflow>(accountTransactResult.Error);
        }

        return transaction;
    }

    public static Result<Inflow> CreateForeign(
        Account account,
        Money convertedAmount,
        Money originalAmount,
        DateTime transactedAt,
        Payee payer)
    {
        if (account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Inflow>(TransactionErrors.TransactionIsNotForeign);
        }

        var transaction = new Inflow(account, convertedAmount, transactedAt, payer, originalAmount);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Inflow>(accountTransactResult.Error);
        }

        return transaction;
    }
}