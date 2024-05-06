using System.Linq.Expressions;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public interface ISubcategoryRepository
{
    Task<Result<IEnumerable<Subcategory>>> GetWhereAsync(
        Expression<Func<Subcategory, bool>> filterPredicate,
        CancellationToken cancellationToken);

    Task<Result> RemoveRangeAsync(IEnumerable<Subcategory> subcategories, CancellationToken cancellationToken);

    Task<Result> UpdateRangeAsync(IEnumerable<Subcategory> subcategories, CancellationToken cancellationToken);

    Task<Result<Subcategory>> AddAsync(Subcategory subcategory, CancellationToken cancellationToken);

    Task<Result<Subcategory>> GetByIdAsync(SubcategoryId subcategoryId, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(Subcategory subcategory, CancellationToken cancellationToken);

    Task<Result<Subcategory>> UpdateAsync(Subcategory subcategory, CancellationToken cancellationToken);
}