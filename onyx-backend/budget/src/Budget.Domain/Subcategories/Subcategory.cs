using Abstractions.DomainBaseTypes;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;
using System.Transactions;
using Models.DataTypes;
using Transaction = Budget.Domain.Transactions.Transaction;

namespace Budget.Domain.Subcategories;

public sealed class Subcategory : Entity<SubcategoryId>
{
    public SubcategoryName Name { get; private set; }
    public SubcategoryDescription? Description { get; private set; }
    private readonly List<Assignment> _assignments;
    public IReadOnlyCollection<Assignment> Assignments => _assignments;
    public Target? Target { get; private set; }

    private Subcategory(SubcategoryName name, SubcategoryDescription? description, List<Assignment> assignments, Target? target)
    {
        Name = name;
        Description = description;
        _assignments = assignments;
        Target = target;
    }

    internal static Result<Subcategory> Create(string name)
    {
        var subcategoryNameCreateResult = SubcategoryName.Create(name);

        if (subcategoryNameCreateResult.IsFailure)
        {
            return Result.Failure<Subcategory>(subcategoryNameCreateResult.Error);
        }

        var subcategoryName = subcategoryNameCreateResult.Value;

        return new Subcategory(subcategoryName, null, new List<Assignment>(), null);
    }

    public Result ChangeName(string name)
    {
        var subcategoryNameCreateResult = SubcategoryName.Create(name);

        if (subcategoryNameCreateResult.IsFailure)
        {
            return Result.Failure(subcategoryNameCreateResult.Error);
        }

        var subcategoryName = subcategoryNameCreateResult.Value;

        Name = subcategoryName;

        return Result.Success();
    }

    public Result ChangeDescription(string description)
    {
        var descriptionCreateResult = SubcategoryDescription.Create(description);

        if (descriptionCreateResult.IsFailure)
        {
            return Result.Failure(descriptionCreateResult.Error);
        }

        var subcategoryDescription = descriptionCreateResult.Value;

        Description = subcategoryDescription;

        return Result.Success();
    }

    public Result<Assignment> Assign(int month, int year, Money amount)
    {
        var monthDateCreateResult = MonthDate.Create(month, year);

        if (monthDateCreateResult.IsFailure)
        {
            return Result.Failure<Assignment>(monthDateCreateResult.Error);
        }

        var monthDate = monthDateCreateResult.Value;
        var isAssignedForMonth = _assignments.Any(x => x.Month == monthDate);

        if (isAssignedForMonth)
        {
            return Result.Failure<Assignment>(SubcategoryErrors.SubcategoryAlreadyAssignedForMonth);
        }

        var assignmentCreateResult = Assignment.Create(monthDate, amount);

        if (assignmentCreateResult.IsFailure)
        {
            return Result.Failure<Assignment>(assignmentCreateResult.Error);
        }

        var assignment = assignmentCreateResult.Value;
        _assignments.Add(assignment);

        return Result.Success(assignment);
    }

    public Result Unassign(int month, int year)
    {
        var monthDateCreateResult = MonthDate.Create(month, year);

        if (monthDateCreateResult.IsFailure)
        {
            return Result.Failure<Assignment>(monthDateCreateResult.Error);
        }

        var monthDate = monthDateCreateResult.Value;
        var assignment = _assignments.FirstOrDefault(x => x.Month == monthDate);

        if (assignment is null)
        {
            return Result.Failure<Assignment>(SubcategoryErrors.SubcategoryNotAssignedForMonth);
        }

        _assignments.Remove(assignment);

        return Result.Success();
    }

    public Result<Assignment> Reassign(int month, int year, Money amount)
    {
        var monthDateCreateResult = MonthDate.Create(month, year);

        if (monthDateCreateResult.IsFailure)
        {
            return Result.Failure<Assignment>(monthDateCreateResult.Error);
        }

        var monthDate = monthDateCreateResult.Value;
        var assignment = _assignments.FirstOrDefault(x => x.Month == monthDate);

        if (assignment is null)
        {
            return Result.Failure<Assignment>(SubcategoryErrors.SubcategoryNotAssignedForMonth);
        }

        var reassignResult = assignment.ChangeAssignedAmount(amount);

        if (reassignResult.IsFailure)
        {
            return Result.Failure<Assignment>(reassignResult.Error);
        }

        return assignment;
    }

    internal Result<Assignment> Transact(Transaction transaction)
    {
        var transactionMonthDateCreateResult = MonthDate.Create(
            transaction.TransactedAt.Month,
            transaction.TransactedAt.Year);

        if (transactionMonthDateCreateResult.IsFailure)
        {
            return Result.Failure<Assignment>(transactionMonthDateCreateResult.Error);
        }

        var transactionMonthDate = transactionMonthDateCreateResult.Value;
        var assignment = _assignments.FirstOrDefault(a => a.Month == transactionMonthDate);

        if (assignment is null)
        {
            return Result.Failure<Assignment>(SubcategoryErrors.SubcategoryNotAssignedForMonth);
        }

        assignment.Transact(transaction);

        Target?.Transact(transaction);

        return Result.Success(assignment);
    }

    public Result RemoveTransaction(Transaction transaction)
    {
        var transactionMonthDateCreateResult = MonthDate.Create(
            transaction.TransactedAt.Month,
            transaction.TransactedAt.Year);

        if (transactionMonthDateCreateResult.IsFailure)
        {
            return Result.Failure<Assignment>(transactionMonthDateCreateResult.Error);
        }

        var transactionMonthDate = transactionMonthDateCreateResult.Value;
        var assignment = _assignments.FirstOrDefault(a => a.Month == transactionMonthDate);

        if (assignment is null)
        {
            return Result.Failure<Assignment>(SubcategoryErrors.SubcategoryNotAssignedForMonth);
        }

        assignment.RemoveTransaction(transaction);

        Target?.RemoveTransaction(transaction);

        return Result.Success(assignment);
    }

    public Result<Target> SetTarget(Money targetAmount, MonthDate upToMonth)
    {
        if (Target is not null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetAlreadySet);
        }

        var targetCreateResult = Target.Create(upToMonth, targetAmount);

        if (targetCreateResult.IsFailure)
        {
            return Result.Failure<Target>(targetCreateResult.Error);
        }

        var target = targetCreateResult.Value;
        Target = target;

        return target;
    }

    public Result UnsetTarget()
    {
        if(Target is null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetNotSet);
        }

        Target = null;

        return Result.Success();
    }

    public Result MoveTargetEndMonth(MonthDate newEndMonth)
    {
        if (Target is null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetNotSet);
        }

        var monthMoveResult = Target.MoveTargetEndMonth(newEndMonth);

        return monthMoveResult;
    }

    public Result UpdateTargetAmount(Money amount)
    {
        if (Target is null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetNotSet);
        }

        var targetAmountUpdateResult = Target.UpdateTargetAmount(amount);

        return targetAmountUpdateResult;
    }
}