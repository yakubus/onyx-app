﻿using Budget.Application.Abstractions.Identity;
using Budget.Domain.Categories;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

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

    public Result<Category> GetByName(CategoryName name, CancellationToken cancellationToken)
    {
        return GetFirst(x => x.Name == name);
    }
}