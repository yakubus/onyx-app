using Budget.Domain.Subcategories;
using Budget.Infrastructure.Data;

namespace Budget.Infrastructure.Repositories;

internal sealed class SubcategoryRepository : Repository<Subcategory, SubcategoryId>, ISubcategoryRepository
{
    public SubcategoryRepository(CosmosDbContext context) : base(context)
    {
    }
}