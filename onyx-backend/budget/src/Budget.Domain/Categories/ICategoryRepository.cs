using Models.Responses;
using System.Linq.Expressions;

namespace Budget.Domain.Categories;

public interface ICategoryRepository
{
    Task<Result<Category>> GetByNameAsync(CategoryName name, CancellationToken cancellationToken);

    Task<Result<Category>> AddAsync(Category category, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<Category>> GetSingleAsync(
        Expression<Func<Category, bool>> filterPredicate,
        CancellationToken cancellationToken);

    Task<Result<Category>> GetByIdAsync(Guid requestCategoryId, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(CategoryId categoryId, CancellationToken cancellationToken);

    Task<Result<Category>> UpdateAsync(Category category, CancellationToken cancellationToken);
}