using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Domain.Categories;

public interface ICategoryRepository
{
    Task<Result<Category>> GetByNameAsync(CategoryName name, CancellationToken cancellationToken);

    Task<Result<Category>> AddAsync(Category category, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result<Category>> GetByIdAsync(CategoryId categoryId, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(CategoryId categoryId, CancellationToken cancellationToken = default);

    Task<Result<Category>> UpdateAsync(Category category, CancellationToken cancellationToken);

    Task<Result<Category>> GetCategoryWithSubcategory(SubcategoryId subcategoryId, CancellationToken cancellationToken);
}