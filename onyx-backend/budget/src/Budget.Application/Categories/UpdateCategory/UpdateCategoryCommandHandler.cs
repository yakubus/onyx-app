using Abstractions.Messaging;
using Budget.Application.Categories.Models;
using Budget.Domain.Categories;
using Models.Responses;

namespace Budget.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryModel>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryGetResult = await _categoryRepository.GetByIdAsync(new (request.Id), cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return Result.Failure<CategoryModel>(categoryGetResult.Error);
        }

        var category = categoryGetResult.Value;

        var categoryChangeNameResult = category.ChangeName(request.NewName);

        if (categoryChangeNameResult.IsFailure)
        {
            return Result.Failure<CategoryModel>(categoryChangeNameResult.Error);
        }

        var categoryUpdateResult = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (categoryUpdateResult.IsFailure)
        {
            return Result.Failure<CategoryModel>(categoryUpdateResult.Error);
        }

        category = categoryUpdateResult.Value;

        return CategoryModel.FromDomainModel(category);
    }
}