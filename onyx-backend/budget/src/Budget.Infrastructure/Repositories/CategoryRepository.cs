using Abstractions;
using Budget.Domain.Categories;
using Models.Responses;
using SharedDAL;

namespace Budget.Infrastructure.Repositories;

internal sealed class CategoryRepository : Repository<Category, CategoryId>, ICategoryRepository
{
    public CategoryRepository(CosmosDbContext context) : base(context)
    {
    }

    public async Task<Result<Category>> GetByNameAsync(CategoryName name, CancellationToken cancellationToken)
    {
        var entities = await Task.Run(
            () => Container.GetItemLinqQueryable<Category>(true)
                .Where(c => c.Name == name)
                .AsEnumerable(),
            cancellationToken);

        var entity = entities.SingleOrDefault();

        return entity is null ?
            Result.Failure<Category>(DataAccessErrors<Category>.NotFound) :
            Result.Success(entity);
    }
}