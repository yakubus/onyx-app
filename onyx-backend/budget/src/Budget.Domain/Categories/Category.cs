using System.Text.Json.Serialization;
using Abstractions.DomainBaseTypes;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Domain.Categories;

public sealed class Category : Entity<CategoryId>
{
    public CategoryName Name { get; private set; }
    private readonly List<Subcategory> _subcategories;
    public IReadOnlyCollection<Subcategory> Subcategories => _subcategories;
    private const int maxSubcategoriesCount = 10;

    private Category(CategoryName name, List<Subcategory> subcategories)
    {
        Name = name;
        _subcategories = subcategories;
    }

    public static Result<Category> Create(string name)
    {
        var categoryNameCreateResult = CategoryName.Create(name);

        if (categoryNameCreateResult.IsFailure)
        {
            return Result.Failure<Category>(categoryNameCreateResult.Error);
        }

        var categoryName = categoryNameCreateResult.Value;

        return new Category(categoryName, new ());
    }

    public Result ChangeName(string name)
    {
        var categoryNameCreateResult = CategoryName.Create(name);

        if (categoryNameCreateResult.IsFailure)
        {
            return Result.Failure<Category>(categoryNameCreateResult.Error);
        }

        var categoryName = categoryNameCreateResult.Value;

        Name = categoryName;

        return Result.Success();
    }

    public Result<Subcategory> NewSubcategory(string subcategoryName)
    {
        if (_subcategories.Count >= maxSubcategoriesCount)
        {
            return Result.Failure<Subcategory>(CategoryErrors.MaxSubcategoriesCountReached);
        }

        var subcategoryCreateResult = Subcategory.Create(subcategoryName);

        if (subcategoryCreateResult.IsFailure)
        {
            return Result.Failure<Subcategory>(subcategoryCreateResult.Error);
        }

        var subcategory = subcategoryCreateResult.Value;
        _subcategories.Add(subcategory);

        return Result.Success(subcategory);
    }

    public Result RemoveSubcategory(Subcategory subcategory)
    {
        return _subcategories.Remove(subcategory) ?
            Result.Success() :
            Result.Failure<Category>(CategoryErrors.SubcategoryNotFound);
    }

    public Result 
}