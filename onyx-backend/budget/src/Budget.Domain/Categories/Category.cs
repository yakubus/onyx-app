using Budget.Domain.Budgets;
using Budget.Domain.Shared.Abstractions;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Domain.Categories;

public sealed class Category : BudgetOwnedEntity<CategoryId>
{
    public CategoryName Name { get; private set; }
    private readonly List<SubcategoryId> _subcategoriesId;
    public IReadOnlyCollection<SubcategoryId> SubcategoriesId => _subcategoriesId;
    private const int maxSubcategoriesCount = 10;

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Category(CategoryName name, List<SubcategoryId> subcategoriesId, BudgetId budgetId, CategoryId? id = null) 
        : base(budgetId, id ?? new CategoryId())
    {
        Name = name;
        _subcategoriesId = subcategoriesId;
    }

    public static Result<Category> Create(string name, BudgetId budgetId)
    {
        var categoryNameCreateResult = CategoryName.Create(name);

        if (categoryNameCreateResult.IsFailure)
        {
            return Result.Failure<Category>(categoryNameCreateResult.Error);
        }

        var categoryName = categoryNameCreateResult.Value;

        return new Category(categoryName, new (), budgetId);
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
        if (_subcategoriesId.Count >= maxSubcategoriesCount)
        {
            return Result.Failure<Subcategory>(CategoryErrors.MaxSubcategoriesCountReached);
        }

        var subcategoryCreateResult = Subcategory.Create(subcategoryName);

        if (subcategoryCreateResult.IsFailure)
        {
            return Result.Failure<Subcategory>(subcategoryCreateResult.Error);
        }

        var subcategory = subcategoryCreateResult.Value;

        _subcategoriesId.Add(subcategory.Id);

        return Result.Success(subcategory);
    }

    public Result RemoveSubcategory(Subcategory subcategory)
    {
        return _subcategoriesId.Remove(subcategory.Id) ?
            Result.Success() :
            Result.Failure<Category>(CategoryErrors.SubcategoryNotFound);
    }

}