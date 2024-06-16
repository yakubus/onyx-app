using Budget.Domain.Budgets;
using Budget.Domain.Shared.Abstractions;
using Models.DataTypes;
using Models.Responses;
using Transaction = Budget.Domain.Transactions.Transaction;

namespace Budget.Domain.Subcategories;

public sealed class Subcategory : BudgetOwnedEntity<SubcategoryId>
{
    public SubcategoryName Name { get; private set; }
    public SubcategoryDescription? Description { get; private set; }
    private readonly List<Assignment> _assignments;
    public IReadOnlyCollection<Assignment> Assignments => _assignments;
    public Target? Target { get; private set; }

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Subcategory(
        SubcategoryName name,
        SubcategoryDescription? description,
        IEnumerable<Assignment> assignments,
        Target? target,
        BudgetId budgetId,
        SubcategoryId? id = null) : base(budgetId, id ?? new SubcategoryId())
    {
        Name = name;
        Description = description;
        _assignments = assignments.ToList();
        Target = target;
    }

    internal static Result<Subcategory> Create(string name, BudgetId budgetId)
    {
        var subcategoryNameCreateResult = SubcategoryName.Create(name);

        if (subcategoryNameCreateResult.IsFailure)
        {
            return Result.Failure<Subcategory>(subcategoryNameCreateResult.Error);
        }

        var subcategoryName = subcategoryNameCreateResult.Value;

        return new Subcategory(subcategoryName, null, new List<Assignment>(), null, budgetId);
    }

    internal Result ChangeName(string name)
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

    internal Result ChangeDescription(string description)
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

    internal Result<Assignment> Assign(int month, int year, Money amount)
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

    internal Result<Assignment> Reassign(int month, int year, Money amount)
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

        if (amount.Amount <= 0)
        {
            _assignments.Remove(assignment);
        }

        var reassignResult = assignment.ChangeAssignedAmount(amount);

        if (reassignResult.IsFailure)
        {
            return Result.Failure<Assignment>(reassignResult.Error);
        }

        return assignment;
    }

    internal Result Transact(Transaction transaction)
    {
        var assignment = _assignments.FirstOrDefault(a => a.Month.ContainsDate(transaction.TransactedAt));

        assignment?.Transact(transaction);

        Target?.Transact(transaction);

        return Result.Success();
    }
    public Result TransactForAssignment(Transaction transaction)
    {
        var assignment = _assignments.FirstOrDefault(a => a.Month.ContainsDate(transaction.TransactedAt));

        assignment?.Transact(transaction);

        return Result.Success();
    }

    public Result TransactForTarget(Transaction transaction)
    {
        Target?.Transact(transaction);

        return Result.Success();
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

    public Result<Target> SetTarget(Money targetAmount, MonthDate startedAt, MonthDate upToMonth)
    {
        if (Target is not null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetAlreadySet);
        }

        var targetCreateResult = Target.Create(startedAt, upToMonth, targetAmount);

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

    internal Result MoveTargetEndMonth(MonthDate newEndMonth)
    {
        if (Target is null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetNotSet);
        }

        var monthMoveResult = Target.MoveTargetEndMonth(newEndMonth);

        return monthMoveResult;
    }

    internal Result UpdateTargetAmount(Money amount)
    {
        if (Target is null)
        {
            return Result.Failure<Target>(SubcategoryErrors.TargetNotSet);
        }

        var targetAmountUpdateResult = Target.UpdateTargetAmount(amount);

        return targetAmountUpdateResult;
    }

    public Assignment? GetAssignmentForDate(DateTime transactedAt)
    {
        var monthDateCreateResult = MonthDate.FromDateTime(transactedAt);

        return monthDateCreateResult.IsFailure ? null : GetAssignmentForMonth(monthDateCreateResult.Value);
    }

    public Assignment? GetAssignmentForMonth(MonthDate month) =>
        _assignments.Find(a => a.Month == month);
}