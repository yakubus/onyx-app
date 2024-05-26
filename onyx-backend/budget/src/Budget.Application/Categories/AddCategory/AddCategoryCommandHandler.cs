using Abstractions.Messaging;
using Budget.Application.Categories.Models;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Categories.AddCategory;

internal sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand, CategoryModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public AddCategoryCommandHandler(ICategoryRepository categoryRepository, ISubcategoryRepository subcategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    // TODO: Add max categories validation (10 per budget (increased by 3 for each budget member))
    public async Task<Result<CategoryModel>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryCreateResult = Category.Create(request.Name);

        if (categoryCreateResult.IsFailure)
        {
            return Result.Failure<CategoryModel>(categoryCreateResult.Error);
        }

        var category = categoryCreateResult.Value;
        var categoryIsNotUniqueResult = await _categoryRepository.GetByNameAsync(category.Name, cancellationToken);

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