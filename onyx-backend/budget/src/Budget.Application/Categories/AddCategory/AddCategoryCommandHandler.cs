using Abstractions.Messaging;
using Budget.Application.Categories.Models;
using Budget.Domain.Budgets;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Categories.AddCategory;

internal sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBudgetRepository _budgetRepository;

    public AddCategoryCommandHandler(ICategoryRepository categoryRepository, IBudgetRepository budgetRepository)
    {
        _categoryRepository = categoryRepository;
        _budgetRepository = budgetRepository;
    }

    // TODO: Add max categories validation (20 per budget (increased by 3 for each budget member))
    public async Task<Result<CategoryModel>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {

        var budgetGetResult = await _budgetRepository.GetCurrentBudgetAsync(cancellationToken);

        if (budgetGetResult.IsFailure)
        {
            return budgetGetResult.Error;
        }

        var budget = budgetGetResult.Value;

        var categoryCreateResult = Category.Create(request.Name, new(request.BudgetId));

        if (categoryCreateResult.IsFailure)
        {
            return Result.Failure<CategoryModel>(categoryCreateResult.Error);
        }

        var category = categoryCreateResult.Value;
        var categoryIsNotUniqueResult = _categoryRepository.GetByName(category.Name, cancellationToken);

        if (categoryIsNotUniqueResult.IsSuccess)
        {
            return Result.Failure<CategoryModel>(AddCategoryErrors.CategoryAlreadyExistsError);
        }

        var categoryAddResult = await _categoryRepository.AddAsync(category, cancellationToken);

        if (categoryAddResult.IsFailure)
        {
            return Result.Failure<CategoryModel>(categoryAddResult.Error);
        }

        category = categoryAddResult.Value;

        return CategoryModel.FromDomainModel(category, Array.Empty<Subcategory>());
    }
}