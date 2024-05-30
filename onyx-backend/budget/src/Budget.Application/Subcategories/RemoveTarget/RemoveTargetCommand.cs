using Abstractions.Messaging;
using Budget.Application.Abstractions.Messaging;
using Budget.Application.Subcategories.Models;

namespace Budget.Application.Subcategories.RemoveTarget;

public sealed record RemoveTargetCommand(Guid SubcategoryId, Guid BudgetId) : BudgetCommand<SubcategoryModel>(BudgetId)
{
}