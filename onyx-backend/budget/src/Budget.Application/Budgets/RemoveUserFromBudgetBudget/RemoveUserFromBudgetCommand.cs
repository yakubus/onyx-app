using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.UpdateBudget;

public sealed record RemoveUserFromBudgetCommand(Guid BudgetId, string UserIdToRemove) : ICommand<BudgetModel>;