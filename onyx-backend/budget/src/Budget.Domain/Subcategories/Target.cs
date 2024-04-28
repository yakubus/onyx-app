using Abstractions.DomainBaseTypes;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public sealed record Target : ValueObject
{
    public MonthDate UpToMonth { get; private set; }
    public MonthDate StartedAt { get; init; }
    public Money TargetAmount { get; init; }
    public Money CollectedAmount { get; private set; }
    public Money AmountAssignedEveryMonth =>
        MonthDate.MonthsInterval(UpToMonth, MonthDate.Current) is var monthsInterval && monthsInterval <= 0 ?
            TargetAmount with { Amount = 0 } :
            (TargetAmount - CollectedAmount) / monthsInterval ;

    //TODO: Add Target type (recurring or one-time)

    //TODO: Add Target collecting policy:
    // ------------ AVAILABLE FOR RECURRING ------------
    // - set aside target amount when target ends
    // - refill up to previous state
    // ------------ AVAILABLE FOR ONE-TIME ------------
    // - end the target

    private Target(MonthDate upToMonth, Money targetAmount, Money collectedAmount, MonthDate startedAt)
    {
        UpToMonth = upToMonth;
        TargetAmount = targetAmount;
        CollectedAmount = collectedAmount;
        StartedAt = startedAt;
    }

    internal static Result<Target> Create(MonthDate upToMonth, Money targetAmount)
    {
        if (targetAmount.Amount <= 0)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetAmountMustBePositive);
        }

        if (upToMonth <= MonthDate.Current)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetDateMustBeInNextOrCurrentMonth);
        }

        return new Target(
            upToMonth, 
            targetAmount, 
            targetAmount with { Amount = 0 }, 
            MonthDate.Current);
    }

    internal Result MoveTargetEndMonth(MonthDate upToMonth)
    {
        if (upToMonth <= MonthDate.Current)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetDateMustBeInNextOrCurrentMonth);
        }

        UpToMonth = upToMonth;

        return Result.Success();
    }

    internal Result Transact(Transaction transaction)
    {
        if (!UpToMonth.ContainsDate(transaction.TransactedAt))
        {
            return Result.Failure(SubcategoryErrors.TargetDateHasPassed);
        }

        CollectedAmount += transaction.Amount with { Amount = Math.Abs(transaction.Amount.Amount) };

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

        CollectedAmount -= transaction.Amount with { Amount = Math.Abs(transaction.Amount.Amount) };

        return Result.Success();
    }
}