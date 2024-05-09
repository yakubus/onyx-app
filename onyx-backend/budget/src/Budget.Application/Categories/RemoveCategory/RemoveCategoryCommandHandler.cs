using Abstractions.Messaging;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Categories.RemoveCategory;

internal sealed class RemoveCategoryCommandHandler : ICommandHandler<RemoveCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveCategoryCommandHandler(ICategoryRepository categoryRepository, ISubcategoryRepository subcategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryGetResult = await _categoryRepository.GetByIdAsync(new (request.CategoryId), cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return Result.Failure(categoryGetResult.Error);
        }

        var category = categoryGetResult.Value;
        var subcategoriesToRemove = category.Subcategories;

        return await RemoveCategoryAndAssociates(subcategoriesToRemove, category, cancellationToken);
    }

    private async Task<Result> RemoveCategoryAndAssociates(
        IReadOnlyCollection<Subcategory> subcategoriesToRemove,
        Category category,
        CancellationToken cancellationToken)
    {
        var subcategoriesRemoveTask = _subcategoryRepository.RemoveRangeAsync(subcategoriesToRemove, cancellationToken);
        var categoryRemoveTask = _categoryRepository.RemoveAsync(category.Id, cancellationToken);

        var results = await Task.WhenAll(subcategoriesRemoveTask, categoryRemoveTask);

        var failureResult = results.FirstOrDefault(x => x.IsFailure);

        return failureResult ?? Result.Success();
    }
}