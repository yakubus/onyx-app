using System.Linq.Expressions;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public interface ISubcategoryRepository
{
    Task<Result<IEnumerable<Subcategory>>> GetFilteredSubcategoriesAsync(
        Expression<Func<Subcategory, bool>> filterPredicate,
        CancellationToken cancellationToken);

    Task<Result> RemoveRangeAsync(IEnumerable<Subcategory> subcategories, CancellationToken cancellationToken);

    Task<Result> UpdateRangeAsync(IEnumerable<Subcategory> subcategories, CancellationToken cancellationToken);
}