using Abstractions.DomainBaseTypes;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions;

public sealed class Transaction : Entity<TransactionId>
{
    public Account Account { get; init; }
    public Money Amount { get; init; }
    public Money? OriginalAmount { get; init; }
    public DateTime TransactedAt { get; init; }
    public Subcategory? Subcategory { get; private set; }
    public Counterparty Counterparty { get; private set; }

    private Transaction(
        Account account,
        Subcategory? subcategory,
        Money amount,
        Money? originalAmount,
        DateTime transactedAt,
        Counterparty counterparty)
    {
        Account = account;
        Amount = amount;
        OriginalAmount = originalAmount;
        TransactedAt = transactedAt;
        Subcategory = subcategory;
        Counterparty = counterparty;
    }

    public Result UpdateSubcategory(Subcategory subcategory)
    {
        if (Subcategory is null)
        {
            return Result.Failure(TransactionErrors.CannotUpdateSubcategoryForUncategorizedTransactionError);
        }

        Subcategory = subcategory;

        return Result.Success();
    }

    public Result UpdateCounterparty(Counterparty counterparty)
    {
        if (Counterparty.Type != counterparty.Type)
        {
            return Result.Failure(TransactionErrors.CannotUpdateCounterpartyWithDifferentType);
        }

        Counterparty = counterparty;

        return Result.Success();
    }

    public static Result<Transaction> CreatePrincipalOutflow(
        Account account,
        Subcategory subcategory,
        Money amount,
        DateTime transactedAt,
        Counterparty payee)
    {
        if (payee.Type != CounterpartyType.Payee)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        var transaction = new Transaction(account, subcategory, amount, null, transactedAt, payee);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        var subcategoryTransactResult = subcategory.Transact(transaction);

        if (subcategoryTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(subcategoryTransactResult.Error);
        }

        return transaction;
    }

    public static Result<Transaction> CreateForeignOutflow(
        Account account,
        Subcategory subcategory,
        Money convertedAmount,
        Money originalAmount,
        DateTime transactedAt,
        Counterparty payee)
    {
        if (payee.Type != CounterpartyType.Payee)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        if (account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionIsNotForeign);
        }

        var transaction = new Transaction(
            account,
            subcategory,
            convertedAmount,
            originalAmount,
            transactedAt,
            payee);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        var subcategoryTransactResult = subcategory.Transact(transaction);

        if (subcategoryTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(subcategoryTransactResult.Error);
        }

        return transaction;
    }

    public static Result<Transaction> CreatePrincipalInflow(
        Account account,
        Money amount,
        DateTime transactedAt,
        Counterparty payer)
    {
        if (payer.Type != CounterpartyType.Payer)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        var transaction = new Transaction(account, null, amount, null, transactedAt, payer);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        return transaction;
    }

    public static Result<Transaction> CreateForeignInflow(
        Account account,
        Money convertedAmount,
        Money originalAmount,
        DateTime transactedAt,
        Counterparty payer)
    {
        if (payer.Type != CounterpartyType.Payer)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        if (account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionIsNotForeign);
        }

        var transaction = new Transaction(account, null, convertedAmount, originalAmount, transactedAt, payer);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        return transaction;
    }
}