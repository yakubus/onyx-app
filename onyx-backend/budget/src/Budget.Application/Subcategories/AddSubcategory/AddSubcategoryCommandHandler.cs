using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.AddSubcategory;

internal sealed class AddSubcategoryCommandHandler : ICommandHandler<AddSubcategoryCommand, SubcategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public AddSubcategoryCommandHandler(
        ICategoryRepository categoryRepository,
        ISubcategoryRepository subcategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result<SubcategoryModel>> Handle(
        AddSubcategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryGetResult = await _categoryRepository.GetByIdAsync(request.ParentCategoryId, cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(categoryGetResult.Error);
        }

        var category = categoryGetResult.Value;

        var subcategoryAddResult = category.NewSubcategory(request.Name);

        if (subcategoryAddResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryAddResult.Error);
        }

        var subcategory = subcategoryAddResult.Value;

        subcategoryAddResult = await _subcategoryRepository.AddAsync(subcategory, cancellationToken);

        if (subcategoryAddResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryAddResult.Error);
        }

        var categoryUpdateResult = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (categoryUpdateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(categoryUpdateResult.Error);
        }

        subcategory = subcategoryAddResult.Value;

        return SubcategoryModel.FromDomainModel(subcategory);
    }
}