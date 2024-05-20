using Abstractions.DomainBaseTypes;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions;

public sealed class Transaction : Entity<TransactionId>
{
    public AccountId AccountId { get; init; }
    public Money Amount { get; init; }
    public Money? OriginalAmount { get; init; }
    public Money? AssignmentAmount { get; private set; }
    public Money? TargetAmount { get; private set; }
    public DateTime TransactedAt { get; init; }
    public SubcategoryId? SubcategoryId { get; private set; }
    public CounterpartyId? CounterpartyId { get; private set; }

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Transaction(
        AccountId accountId,
        Money amount,
        Money? originalAmount,
        DateTime transactedAt,
        SubcategoryId? subcategoryId,
        CounterpartyId? counterpartyId,
        Money? assignmentAmount,
        Money? targetAmount,
        TransactionId? id) : base(id ?? new TransactionId())
    {
        AccountId = accountId;
        Amount = amount;
        OriginalAmount = originalAmount;
        TransactedAt = transactedAt;
        SubcategoryId = subcategoryId;
        CounterpartyId = counterpartyId;
        AssignmentAmount = assignmentAmount;
        TargetAmount = targetAmount;
    }

    private Transaction(
        Account account,
        Subcategory? subcategory,
        Money amount,
        Money? originalAmount,
        DateTime transactedAt,
        Counterparty counterparty,
        Money? assignmentAmount,
        Money? targetAmount,
        TransactionId? id = null) 
        : base(id ?? new TransactionId())
    {
        AccountId = account.Id;
        Amount = amount;
        OriginalAmount = originalAmount;
        TransactedAt = transactedAt;
        AssignmentAmount = assignmentAmount;
        TargetAmount = targetAmount;
        SubcategoryId = subcategory?.Id;
        CounterpartyId = counterparty.Id;
    }

    internal static Result<Transaction> CreatePrincipalOutflow(
        Account account,
        Subcategory subcategory,
        Money amount,
        DateTime transactedAt,
        Counterparty payee,
        Money assignmentAmount,
        Money targetAmount)
    {
        if (payee.Type != CounterpartyType.Payee)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        if (transactedAt.ToUniversalTime() > DateTime.UtcNow)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionCannotBeInFuture);
        }

        var transaction = new Transaction(
            account,
            subcategory,
            amount,
            null,
            transactedAt,
            payee,
            assignmentAmount,
            targetAmount);

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

    internal static Result<Transaction> CreateForeignOutflow(
        Account account,
        Subcategory subcategory,
        Money convertedAmount,
        Money originalAmount,
        DateTime transactedAt,
        Counterparty payee,
        Money assignmentAmount,
        Money targetAmount)
    {
        if (payee.Type != CounterpartyType.Payee)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        if (account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionIsNotForeign);
        }

        if (transactedAt.ToUniversalTime() > DateTime.UtcNow)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionCannotBeInFuture);
        }

        var transaction = new Transaction(
            account,
            subcategory,
            convertedAmount,
            originalAmount,
            transactedAt,
            payee,
            assignmentAmount,
            targetAmount);

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

    internal static Result<Transaction> CreatePrincipalInflow(
        Account account,
        Money amount,
        DateTime transactedAt,
        Counterparty payer)
    {
        if (payer.Type != CounterpartyType.Payer)
        {
            return Result.Failure<Transaction>(TransactionErrors.InvalidCounterpartyType);
        }

        if (transactedAt.ToUniversalTime() > DateTime.UtcNow)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionCannotBeInFuture);
        }

        var transaction = new Transaction(
            account,
            null,
            amount,
            null,
            transactedAt,
            payer,
            null,
            null);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        return transaction;
    }

    internal static Result<Transaction> CreateForeignInflow(
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

        if (transactedAt.ToUniversalTime() > DateTime.UtcNow)
        {
            return Result.Failure<Transaction>(TransactionErrors.TransactionCannotBeInFuture);
        }

        var transaction = new Transaction(
            account,
            null,
            convertedAmount,
            originalAmount,
            transactedAt,
            payer,
            null,
            null);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Transaction>(accountTransactResult.Error);
        }

        return transaction;
    }

    public Result RemoveCounterparty()
    {
        CounterpartyId = null;

        return Result.Success();
    }

    public Result RemoveSubcategory()
    {
        SubcategoryId = null;

        return Result.Success();
    }


    public Result SetAssignmentAmount(Money amount)
    {
        if (amount > 0)
        {
            return TransactionErrors.AssignmentAmountMustBeNegative;
        }

        AssignmentAmount = amount;

        return Result.Success();
    }
    public Result SetTargetAmount(Money amount)
    {
        if (amount > 0)
        {
            return TransactionErrors.TargetAmountMustBeNegative;
        }

        TargetAmount = amount;

        return Result.Success();
    }
}