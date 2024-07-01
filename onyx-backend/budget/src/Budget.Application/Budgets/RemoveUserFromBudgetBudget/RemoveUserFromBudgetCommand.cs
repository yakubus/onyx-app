using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.RemoveUserFromBudgetBudget;

public sealed record RemoveUserFromBudgetCommand(Guid BudgetId, string UserIdToRemove) : ICommand<BudgetModel>;