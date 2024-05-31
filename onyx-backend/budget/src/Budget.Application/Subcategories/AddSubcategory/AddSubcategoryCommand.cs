using Budget.Application.Abstractions.Messaging;
using Budget.Application.Subcategories.Models;

namespace Budget.Application.Subcategories.AddSubcategory;

public sealed record AddSubcategoryCommand(
    Guid ParentCategoryId,
    string Name,
    Guid BudgetId) : BudgetCommand<SubcategoryModel>(BudgetId)
{
}