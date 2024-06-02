using Budget.Application.Abstractions.Identity;
using Budget.Domain.Subcategories;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Repositories;

internal sealed class SubcategoryRepository : BaseBudgetRepository<Subcategory, SubcategoryId>, ISubcategoryRepository
{
    public SubcategoryRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<Subcategory> dataModelService) : base(
        context,
        budgetContext,
        dataModelService)
    {
    }
}