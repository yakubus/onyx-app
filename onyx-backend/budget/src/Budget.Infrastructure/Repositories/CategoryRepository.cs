using Budget.Application.Abstractions.Identity;
using Budget.Domain.Categories;
using Models.Responses;
using SharedDAL;

namespace Budget.Infrastructure.Repositories;

internal sealed class CategoryRepository : BaseBudgetRepository<Category, CategoryId>, ICategoryRepository
{
    public CategoryRepository(CosmosDbContext context, IBudgetContext budgetContext) : base(context, budgetContext)
    {
    }

    public Result<Category> GetByName(CategoryName name, CancellationToken cancellationToken)
    {
        return GetFirst(x => x.Name == name);
    }
}