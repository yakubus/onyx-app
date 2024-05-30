using Abstractions.Messaging;
using Budget.Application.Categories.Models;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Categories.GetCategories;

internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IEnumerable<CategoryModel>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, ISubcategoryRepository subcategoryRepository)
    {
        _categoryRepository = categoryRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryModel>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoriesGetResult = await _categoryRepository.GetAllAsync(cancellationToken);

        if(categoriesGetResult.IsFailure)
        {
            return Result.Failure<IEnumerable<CategoryModel>>(categoriesGetResult.Error);
        }

        var categories = categoriesGetResult.Value.ToList();

        var subcategoriesGetResult = await _subcategoryRepository.GetManyByIdAsync(
            categories.SelectMany(c => c.SubcategoriesId).ToList(),
            cancellationToken);

        if (subcategoriesGetResult.IsFailure)
        {
            return Result.Failure<IEnumerable<CategoryModel>>(subcategoriesGetResult.Error);
        }

        var subcategories = subcategoriesGetResult.Value;

        var categorySubcategoryDictionary = categories.ToDictionary(
            category =>
                category,
            category => subcategories
                .Where(subcategory => category.SubcategoriesId.Contains(subcategory.Id)));

        return Result.Create(
            categorySubcategoryDictionary
                .Select(
                    pair =>
                        CategoryModel.FromDomainModel(pair.Key, pair.Value)));
    }
}