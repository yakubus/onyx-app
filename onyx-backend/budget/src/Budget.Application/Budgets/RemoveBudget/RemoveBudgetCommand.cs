using Abstractions.Messaging;

namespace Budget.Application.Budgets.RemoveBudget;

public sealed record RemoveBudgetCommand(Guid BudgetId) : ICommand
{
}