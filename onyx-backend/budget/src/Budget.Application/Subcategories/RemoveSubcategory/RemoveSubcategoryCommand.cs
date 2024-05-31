using Budget.Application.Abstractions.Messaging;

namespace Budget.Application.Subcategories.RemoveSubcategory;

public sealed record RemoveSubcategoryCommand(Guid Id, Guid BudgetId) : BudgetCommand(BudgetId)
{
}