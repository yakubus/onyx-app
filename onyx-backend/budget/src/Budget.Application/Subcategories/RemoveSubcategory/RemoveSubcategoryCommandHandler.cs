using Abstractions.Messaging;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.RemoveSubcategory;

internal sealed class RemoveSubcategoryCommandHandler : ICommandHandler<RemoveSubcategoryCommand>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ICategoryRepository _categoryRepository;

    public RemoveSubcategoryCommandHandler(ISubcategoryRepository subcategoryRepository, ICategoryRepository categoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(RemoveSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.Id);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var categoryGetResult = await _categoryRepository.GetSingleAsync(
            category => category.Subcategories.Any(s => s.Id == subcategory.Id), 
            cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return Result.Failure(categoryGetResult.Error);
        }

        var category = categoryGetResult.Value;

        var categoryRemoveSubcategoryResult = category.RemoveSubcategory(subcategory);

        if (categoryRemoveSubcategoryResult.IsFailure)
        {
            return Result.Failure(categoryRemoveSubcategoryResult.Error);
        }

        var categoryUpdateResult = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (categoryUpdateResult.IsFailure)
        {
            return Result.Failure(categoryUpdateResult.Error);
        }

        var subcategoryRemoveResult = await _subcategoryRepository.RemoveAsync(subcategory, cancellationToken);

        if (subcategoryRemoveResult.IsFailure)
        {
            return Result.Failure(subcategoryRemoveResult.Error);
        }

        return Result.Success();
    }
}