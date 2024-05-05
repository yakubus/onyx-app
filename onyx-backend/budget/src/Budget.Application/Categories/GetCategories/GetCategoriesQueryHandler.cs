using Abstractions.Messaging;
using Budget.Application.Categories.Models;
using Budget.Domain.Categories;
using Models.Responses;

namespace Budget.Application.Categories.GetCategories;

internal sealed class GetCategoriesQueryHandler : ICommandHandler<GetCategoriesQuery, IEnumerable<CategoryModel>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryModel>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoriesGetResult = await _categoryRepository.GetAllAsync(cancellationToken);

        if(categoriesGetResult.IsFailure)
        {
            return Result.Failure<IEnumerable<CategoryModel>>(categoriesGetResult.Error);
        }

        var categories = categoriesGetResult.Value;

        return Result.Create(categories.Select(CategoryModel.FromDomainModel));
    }
}