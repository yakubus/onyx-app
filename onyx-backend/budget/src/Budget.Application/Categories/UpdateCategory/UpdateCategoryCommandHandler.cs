using Abstractions.Messaging;
using Budget.Application.Categories.Models;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, ISubcategoryRepository subcategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result<CategoryModel>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryGetResult = await _categoryRepository.GetByIdAsync(new (request.Id), cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return categoryGetResult.Error;
        }

        var category = categoryGetResult.Value;

        var categoryChangeNameResult = category.ChangeName(request.NewName);

        if (categoryChangeNameResult.IsFailure)
        {
            return categoryChangeNameResult.Error;
        }

        var categoryIsNotUniqueResult = await _categoryRepository.GetByNameAsync(category.Name, cancellationToken);

        if (categoryIsNotUniqueResult.IsSuccess)
        {
            return UpdateCategoryErrors.CategoryAlreadyExistsError;
        }

        var categoryUpdateResult = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (categoryUpdateResult.IsFailure)
        {
            return categoryUpdateResult.Error;
        }

        category = categoryUpdateResult.Value;

        var subcategoriesGetResult = await _subcategoryRepository.GetManyByIdAsync(
            category.SubcategoriesId,
            cancellationToken);

        if (subcategoriesGetResult.IsFailure)
        {
            return subcategoriesGetResult.Error;
        }

        var subcategories = subcategoriesGetResult.Value;

        return CategoryModel.FromDomainModel(category, subcategories);
    }
}