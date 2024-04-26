using Abstractions.DomainBaseTypes;
using Budget.Domain.Categories;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Subcategories;

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
}