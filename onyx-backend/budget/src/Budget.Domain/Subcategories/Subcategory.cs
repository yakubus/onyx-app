using Abstractions.DomainBaseTypes;
using Budget.Domain.Categories;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Subcategories;

// TODO: Think abount transactions flow inside subcategory (is it ok to persist transactions in Assignment?)
public sealed class Subcategory : Entity<SubcategoryId>
{
    public SubcategoryName Name { get; private set; }
    public SubcategoryDescription? Description { get; private set; }
    private readonly List<Assignment> _assignments;
    public IReadOnlyCollection<Assignment> Assignments => _assignments;

    private Subcategory(SubcategoryName name, SubcategoryDescription? description, List<Assignment> assignments)
    {
        Name = name;
        Description = description;
        _assignments = assignments;
    }

    public static Result<Subcategory> Create(string name)
    {
        var subcategoryNameCreateResult = SubcategoryName.Create(name);

        if (subcategoryNameCreateResult.IsFailure)
        {
            return Result.Failure<Subcategory>(subcategoryNameCreateResult.Error);
        }

        var subcategoryName = subcategoryNameCreateResult.Value;

        return new Subcategory(subcategoryName, null, new List<Assignment>());
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

    // TODO: Assignment related actions
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

        assignment
    }
}