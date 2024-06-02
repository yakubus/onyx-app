using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.UpdateBudget;

public sealed record UpdateBudgetCommand(Guid BudgetId, string? UserIdToAdd, string? UserIdToRemove) : ICommand<BudgetModel>
{
}