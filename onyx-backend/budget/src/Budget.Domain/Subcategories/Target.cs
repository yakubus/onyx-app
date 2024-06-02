using Abstractions.DomainBaseTypes;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public sealed record Target : ValueObject
{
    public MonthDate UpToMonth { get; private set; }
    public MonthDate StartedAt { get; init; }
    public Money TargetAmount { get; private set; }
    public Money CollectedAmount { get; private set; }
    public bool IsActive => MonthDate.Current < UpToMonth;
    public bool IsCompleted => CollectedAmount >= TargetAmount;
    public Money AmountAssignedEveryMonth =>
        MonthDate.MonthsInterval(UpToMonth, MonthDate.Current) is var monthsInterval && monthsInterval <= 0 ?
            TargetAmount with { Amount = 0 } :
            (TargetAmount - CollectedAmount) / monthsInterval ;

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Target(MonthDate upToMonth, Money targetAmount, Money collectedAmount, MonthDate startedAt)
    {
        UpToMonth = upToMonth;
        TargetAmount = targetAmount;
        CollectedAmount = collectedAmount;
        StartedAt = startedAt;
    }

    internal static Result<Target> Create(MonthDate startedAt, MonthDate upToMonth, Money targetAmount)
    {
        if (targetAmount.Amount <= 0)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetAmountMustBePositive);
        }

        if (upToMonth <= MonthDate.Current)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetDateMustBeInFuture);
        }

        return new Target(
            upToMonth, 
            targetAmount, 
            targetAmount with { Amount = 0 }, 
            startedAt);
    }

    internal Result MoveTargetEndMonth(MonthDate upToMonth)
    {
        if (upToMonth <= MonthDate.Current)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetDateMustBeInFuture);
        }

        UpToMonth = upToMonth;

        return Result.Success();
    }

    internal Result UpdateTargetAmount(Money amount)
    {
        if (amount.Amount <= 0)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetAmountMustBePositive);
        }

        TargetAmount = amount;

        return Result.Success();
    }

    internal Result Transact(Transaction transaction)
    {
        if (!UpToMonth.ContainsDate(transaction.TransactedAt))
        {
            return Result.Failure(SubcategoryErrors.TargetDateHasPassed);
        }

        CollectedAmount += transaction.BudgetAmount with { Amount = Math.Abs(transaction.BudgetAmount.Amount) };

        return Result.Success();
    }

    internal Result RemoveTransaction(Transaction transaction)
    {
        if (!UpToMonth.ContainsDate(transaction.TransactedAt))
        {
            return Result.Failure(SubcategoryErrors.TargetDateHasPassed);
        }

        var transactionMonthDateCreateResult = MonthDate.Create(
            transaction.TransactedAt.Month,
            transaction.TransactedAt.Year);

        if (transactionMonthDateCreateResult.IsFailure)
        {
            return Result.Failure(transactionMonthDateCreateResult.Error);
        }

        var transactionMonthDate = transactionMonthDateCreateResult.Value;

        if (transactionMonthDate < StartedAt)
        {
            return Result.Failure(SubcategoryErrors.TargetStartedAfterTransactionDate);
        }

        CollectedAmount -= transaction.BudgetAmount with { Amount = Math.Abs(transaction.BudgetAmount.Amount) };

        return Result.Success();
    }
}