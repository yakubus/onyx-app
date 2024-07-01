using Budget.Application.Abstractions.Identity;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Infrastructure.Data.DataModels.Categories;

namespace Budget.Infrastructure.Repositories;

internal sealed class CategoryRepository : BaseBudgetRepository<Category, CategoryId>, ICategoryRepository
{
    public CategoryRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<Category> dataModelService) : base(
        context,
        budgetContext,
        dataModelService)
    {
    }

    public async Task<Result<Category>> GetByNameAsync(CategoryName name, CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(
            nameof(CategoryDataModel.Name),
            ScanOperator.Equal,
            name.Value);

        return await GetFirstAsync(scanFilter, cancellationToken);
    }

    public async Task<Result<Category>> GetCategoryWithSubcategory(
        SubcategoryId subcategoryId,
        CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(
            nameof(CategoryDataModel.SubcategoriesId),
            ScanOperator.Contains,
            subcategoryId.Value);

        return await GetFirstAsync(scanFilter, cancellationToken);
    }
}