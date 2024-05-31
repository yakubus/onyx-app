using Budget.Application.Abstractions.Messaging;
using Budget.Application.Subcategories.Models;

namespace Budget.Application.Subcategories.UpdateSubcategory;

public sealed record UpdateSubcategoryCommand(
    Guid Id,
    string? NewName,
    string? NewDescription,
    Guid BudgetId) : BudgetCommand<SubcategoryModel>(BudgetId)
{
}