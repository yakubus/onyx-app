using Abstractions.DomainBaseTypes;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties.Payees;
using Budget.Domain.Counterparties.Payers;
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
    public Subcategory? Subcategory { get; init; }
    public Payee? Payee { get; init; }
    public Payer? Payer { get; init; }

    private Transaction(
        Account account,
        Subcategory? subcategory,
        Money amount,
        Money? originalAmount,
        DateTime transactedAt,
        Payee? payee,
        Payer? payer)
    {
        Account = account;
        Amount = amount;
        OriginalAmount = originalAmount;
        TransactedAt = transactedAt;
        Subcategory = subcategory;
        Payee = payee;
        Payer = payer;
    }

    public static Result<Transaction> CreatePrincipalOutflow(
        Account account,
        Subcategory subcategory,
        Money amount,
        DateTime transactedAt,
        Payee payee)
    {
        var transaction = new Transaction(account, subcategory, amount, null, transactedAt, payee, null);

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
        Payee payee)
    {
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
            payee,
            null);

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
        Payer payer)
    {
        var transaction = new Transaction(account, null, amount, null, transactedAt, null, payer);

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
        Payer payer)
    {
        if (account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionIsNotForeign);
        }

        var transaction = new Transaction(account, null, convertedAmount, originalAmount, transactedAt, null, payer);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        return transaction;
    }
}