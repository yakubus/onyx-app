using System.Linq.Expressions;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public interface ISubcategoryRepository
{
    Task<Result<IEnumerable<Subcategory>>> GetWhereAsync(
        Expression<Func<Subcategory, bool>> filterPredicate,
        CancellationToken cancellationToken);
    Task<Result<IEnumerable<Subcategory>>> GetWhereAsync(
        string sqlQuery,
        KeyValuePair<string, object>? parameter,
        CancellationToken cancellationToken);

    Task<Result<IEnumerable<Subcategory>>> GetManyByIdAsync(
        IEnumerable<SubcategoryId> ids,
        CancellationToken cancellationToken = default);

    Task<Result> RemoveRangeAsync(IEnumerable<SubcategoryId> subcategories, CancellationToken cancellationToken);

    Task<Result> UpdateRangeAsync(IEnumerable<Subcategory> subcategories, CancellationToken cancellationToken);

    Task<Result<Subcategory>> AddAsync(Subcategory subcategory, CancellationToken cancellationToken);

    Task<Result<Subcategory>> GetByIdAsync(SubcategoryId subcategoryId, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(SubcategoryId subcategoryId, CancellationToken cancellationToken);

    Task<Result<Subcategory>> UpdateAsync(Subcategory subcategory, CancellationToken cancellationToken);
}