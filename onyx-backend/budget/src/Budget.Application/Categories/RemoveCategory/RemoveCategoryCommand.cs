using Budget.Application.Abstractions.Messaging;

namespace Budget.Application.Categories.RemoveCategory;

public sealed record RemoveCategoryCommand(Guid CategoryId, Guid BudgetId) : BudgetCommand(BudgetId)
{
}